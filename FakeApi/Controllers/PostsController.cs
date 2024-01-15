using FakeApi.Entities;
using FakeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers;

public class PostsController : CrudControllerApiBase<Post>
{
    private readonly IRepository<Comment> _commentRepository;

    public PostsController(IRepository<Post> repository, IRepository<Comment> commentRepository) : base(repository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Create a Post
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Post), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Create([FromBody]Post record)
    {
        var item = Repository.Create(record);
        if (item == null)
            return NotFound();
        return Created($"example.com/{nameof(Post)}s/{item.Id}", item);
    }

    /// <summary>
    /// Get Post by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Get([FromRoute]int id)
    {
        var item = Repository.Get(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Delete a Post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Delete(int id)
    {
        var found = Repository.Delete(id);
        if (!found)
            return NotFound();
        return Ok();
    }

    /// <summary>
    /// Get all Posts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Post[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult GetAll()
    {
        var item = Repository.GetAll();
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }
    
    /// <summary>
    /// Get user comments by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/comments")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetComments([FromRoute] int id)
    {
        var item = _commentRepository.Filter(w => w.PostId == id);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }
}