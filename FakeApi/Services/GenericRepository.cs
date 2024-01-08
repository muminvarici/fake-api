using System.Text.Json;
using System.Timers;
using FakeApi.Entities;
using Timer = System.Timers.Timer;

namespace FakeApi.Services;

// ReSharper disable InconsistentlySynchronizedField
public class GenericRepository<T> : IRepository<T> where T : EntityBase, new()
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadOnlyCollection<T> _items = new List<T>();
    private readonly Dictionary<string, CachedDataHolder<T>> _cachedItems = new();

    private readonly object _lockObject = new();
    private readonly object _holderLock = new();
    private readonly string _fileName = $"Data\\{typeof(T).Name}s.json";

    public GenericRepository(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
        if (!File.Exists(_fileName))
            throw new FileNotFoundException(_fileName);
    }

    private IReadOnlyCollection<T> LoadData()
    {
        var data = new List<T>();
        if (_items.Count != 0)
        {
            LoadUserData(data);
            return data;
        }

        lock (_lockObject)
        {
            if (_items.Count != 0)
            {
                LoadUserData(data);
                return data;
            }

            var fileContent = File.ReadAllText(_fileName);
            var items = JsonSerializer.Deserialize<T[]>(fileContent, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (items == null)
                throw new InvalidOperationException();

            (_items as List<T>)!.AddRange(items);
            LoadUserData(data);
            return data;
        }
    }

    private void LoadUserData(List<T> data)
    {
        IEnumerable<T> removedItems = Array.Empty<T>();
        IEnumerable<T> createdItems = Array.Empty<T>();
        var userId = _currentUser.GetCurrentUserId();

        if (_cachedItems.TryGetValue(userId, out var value))
        {
            removedItems = value.Data
                .Where(w => !w.IsActive)
                .Select(w => w.Data);

            createdItems = value.Data
                .Where(w => w.IsActive)
                .Select(w => w.Data);
        }

        data.AddRange(_items);
        data.AddRange(createdItems);
        data.RemoveAll(w => removedItems.Any(q => q.Id == w.Id));
    }

    public T? Get(int id)
    {
        var item = LoadData()
            .SingleOrDefault(w => w.Id == id);
        return item;
    }

    public IReadOnlyCollection<T> GetAll()
    {
        var items = LoadData();
        return items;
    }

    public IReadOnlyCollection<T> Filter(Func<T, bool> expression)
    {
        var items = LoadData()
            .Where(expression)
            .ToList();

        return items;
    }

    public IReadOnlyCollection<T> Filter(Func<T, bool> expression, int pageSize, int pageNumber)
    {
        var items = LoadData()
            .Where(expression)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageNumber)
            .ToList();

        return items;
    }

    public T Create(T record)
    {
        var items = LoadData();

        var holder = GetHolder();
        lock (_lockObject)
        {
            var id = items.Max(w => w.Id);
            holder.AddData(id + 1, record);
        }

        return record;
    }

    public bool Delete(int id)
    {
        var items = LoadData();
        var userId = _currentUser.GetCurrentUserId();

        var holder = GetHolder();
        var record = items.SingleOrDefault(w => w.Id == id);
        if (record == null)
            return false;

        lock (_lockObject)
        {
            holder.RemoveData(record);
            holder.SetTimer((_, _) =>
            {
                lock (_lockObject)
                {
                    if (!_cachedItems.ContainsKey(userId)) return;
                    _cachedItems.Remove(userId);
                }
            });
        }

        return true;
    }

    private CachedDataHolder<T> GetHolder()
    {
        var userId = _currentUser.GetCurrentUserId();
        lock (_holderLock)
        {
            if (_cachedItems.TryGetValue(userId, out var holder))
                return holder;

            holder = new CachedDataHolder<T>();
            holder.SetTimer((_, _) =>
            {
                lock (_lockObject)
                {
                    if (!_cachedItems.ContainsKey(userId)) return;
                    _cachedItems.Remove(userId);
                }
            });

            _cachedItems.Add(_currentUser.GetCurrentUserId(), holder);
            return holder;
        }
    }
}

internal record CachedDataHolder<T> where T : EntityBase
{
    private readonly List<CachedData<T>> _data = [];
    private Timer? _timer;

    public IReadOnlyCollection<CachedData<T>> Data => _data;


    public void SetTimer(ElapsedEventHandler elapsed)
    {
        _timer ??= new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
        _timer.Stop();
        _timer.Elapsed += elapsed;
        _timer.Start();
    }

    public void AddData(int id, T data)
    {
        data.Id = id;
        _data.Add(new CachedData<T>(data, true));
    }


    public void RemoveData(T data)
    {
        var item = _data.SingleOrDefault(w => w.Data.Id == data.Id);
        if (item == null)
            _data.Add(new CachedData<T>(data, false));
        else
            item.IsActive = false;
    }
}

internal record CachedData<T>(T Data, bool IsActive)
{
    public T Data { get; set; } = Data;
    public bool IsActive { get; set; } = IsActive;
}