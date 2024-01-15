using FakeApi.Entities;
using FakeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers;

public class UsersController : CrudControllerApiBase<User>
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<Album> _albumRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<Photo> _photoRepository;

    public UsersController
    (
        IRepository<User> repository,
        IRepository<Todo> todoRepository,
        IRepository<Album> albumRepository,
        IRepository<Post> postRepository,
        IRepository<Photo> photoRepository
    ) : base(repository)
    {
        _todoRepository = todoRepository;
        _albumRepository = albumRepository;
        _postRepository = postRepository;
        _photoRepository = photoRepository;
    }

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Create([FromBody] User record)
    {
        var item = Repository.Create(record);
        if (item == null)
            return NotFound();
        return Created($"example.com/{nameof(User)}s/{item.Id}", item);
    }

    /// <summary>
    /// Get user by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Get([FromRoute] int id)
    {
        var item = Repository.Get(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Delete([FromRoute] int id)
    {
        var found = Repository.Delete(id);
        if (!found)
            return NotFound();
        return Ok();
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(User[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult GetAll()
    {
        var item = Repository.GetAll();
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get user todos by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/todos")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTodos([FromRoute] int id)
    {
        var user = Repository.Get(id);
        if (user == null)
            return NotFound();
        var item = _todoRepository.Filter(w => w.UserId == id);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get user albums by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/albums")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAlbums([FromRoute] int id)
    {
        var user = Repository.Get(id);
        if (user == null)
            return NotFound();
        var item = _albumRepository.Filter(w => w.UserId == id);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get user posts by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/posts")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPosts([FromRoute] int id)
    {
        var user = Repository.Get(id);
        if (user == null)
            return NotFound();
        var item = _postRepository.Filter(w => w.UserId == id);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get user photos by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="albumId"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/albums/{albumId:int}photos")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetComments([FromRoute] int id, [FromRoute] int albumId)
    {
        //those queries will be applied over joined collections
        var user = Repository.Get(id);
        if (user == null)
            return NotFound();
        var albumCount = _albumRepository.Filter(w => w.UserId == id).Count;
        if (albumCount == 0)
            return NotFound();
        var item = _photoRepository.Filter(w => w.AlbumId == albumId);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }
}