using FakeApi.Entities;
using FakeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers;

public class AlbumsController : CrudControllerApiBase<Album>
{
    private readonly IRepository<Photo> _photoRepository;

    public AlbumsController(IRepository<Album> repository, IRepository<Photo> photoRepository) : base(repository)
    {
        _photoRepository = photoRepository;
    }

    /// <summary>
    /// Create an Album
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Album), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Create([FromBody] Album record)
    {
        var item = Repository.Create(record);
        if (item == null)
            return NotFound();
        return Created($"example.com/{nameof(Album)}s/{item.Id}", item);
    }

    /// <summary>
    /// Get Album by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Album), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult Get([FromRoute] int id)
    {
        var item = Repository.Get(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Delete an Album
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
    /// Get all Albums
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Album[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override IActionResult GetAll()
    {
        var item = Repository.GetAll();
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Get user photos by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/photos")]
    [ProducesResponseType(typeof(Todo[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetComments([FromRoute] int id)
    {
        var item = _photoRepository.Filter(w => w.AlbumId == id);
        if (item.Count == 0)
            return NotFound();
        return Ok(item);
    }


    /// <summary>
    /// Delete a user's photo
    /// </summary>
    /// <param name="id"></param>
    /// <param name="photoId"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}/photos/{photoId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteComment([FromRoute] int id, [FromRoute] int photoId)
    {
        var album = Repository.Get(id);
        if (album == null)
            return NotFound();
        var found = _photoRepository.Delete(photoId);
        if (!found)
            return NotFound();
        return Ok();
    }
}