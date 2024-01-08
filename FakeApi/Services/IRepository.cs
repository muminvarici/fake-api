using FakeApi.Entities;

namespace FakeApi.Services;

public interface IRepository<T> where T : EntityBase
{
    T? Get(int id);
    IReadOnlyCollection<T> GetAll();
    IReadOnlyCollection<T> Filter(Func<T, bool> expression);
    IReadOnlyCollection<T> Filter(Func<T, bool> expression, int pageSize, int pageNumber);
    T? Create(T record);
    bool Delete(int id);
}