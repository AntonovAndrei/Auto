using Auto.API.Models;
using Auto.Core.Entities;
using Auto.Data;
using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelsController : ControllerBase
{
    private readonly IAutoStorage _db;

    public ModelsController(IAutoStorage db)
    {
        this._db = db;
    }

    [HttpGet]
    public IEnumerable<Model> Get()
    {
        return _db.ListModels();
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var vehicleModel = _db.FindModel(id);
        if (vehicleModel == default) return NotFound();
        var resource = vehicleModel.ToDynamic();
        resource._actions = new
        {
            create = new
            {
                href = $"/api/models/{id}",
                type = "application/json",
                method = "POST",
                name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
            }
        };
        return Ok(resource);
    }
}