using System.Net.Mime;
using FakeApi.Entities;
using FakeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeApi.Controllers;

[Route("api/v1/[controller]")]
public abstract class CrudControllerApiBase<T> : ControllerBase where T : EntityBase
{
    protected readonly IRepository<T> Repository;

    public CrudControllerApiBase(IRepository<T> repository)
    {
        Repository = repository;
    }

    public abstract IActionResult Create([FromBody] T record);
    public abstract IActionResult Get(int id);
    public abstract IActionResult Delete(int id);
    public abstract IActionResult GetAll();
}