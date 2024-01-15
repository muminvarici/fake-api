using FakeApi.Entities;
using FakeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers;

public class CommentsController : CrudControllerApiBase<Comment>
{
    public CommentsController(IRepository<Comment> repository) : base(repository)
    {
    }

    /// <summary>
    /// Create a Comment
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Create([FromBody]Comment record)
    {
        var item = Repository.Create(record);
        if (item == null)
            return NotFound();
        return Created($"example.com/{nameof(Comment)}s/{item.Id}", item);
    }

    /// <summary>
    /// Get Comment by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Get([FromRoute]int id)
    {
        var item = Repository.Get(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Delete a Comment
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
    /// Get all Comments
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Comment[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult GetAll()
    {
        var item = Repository.GetAll();
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }
}