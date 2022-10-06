using Auto.API.Models;
using Auto.Core.Entities;
using Auto.Data;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IAutoStorage _db;
    private readonly IBus _bus;

    public VehiclesController(IAutoStorage db, IBus bus)
    {
        this._db = db;
        this._bus = bus;
    }

    const int PAGE_SIZE = 25;

    // GET: api/vehicles
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListVehicles().Skip(index).Take(count)
            .Select(v => v.ToResource());
        var total = _db.CountVehicles();
        var _links = HAL.PaginateAsDynamic("/api/vehicles", index, count, total);
        var result = new
        {
            _links,
            count,
            total,
            index,
            items
        };
        return Ok(result);
    }

    // GET api/vehicles/ABC123
    [HttpGet("{id}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string id)
    {
        var vehicle = _db.FindVehicle(id);
        if (vehicle == default) return NotFound();
        var resource = vehicle.ToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/vehicles/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            }
        };
        return Ok(resource);
    }

    // PUT api/vehicles/ABC123
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] VehicleDto dto)
    {
        var vehicleModel = _db.FindModel(dto.ModelCode);
        var vehicle = new Vehicle
        {
            Registration = dto.Registration,
            Color = dto.Color,
            Year = dto.Year,
            ModelCode = vehicleModel.Code
        };
        _db.UpdateVehicle(vehicle);
        return Ok(dto);
    }
    
    // POST api/vehicles
    [HttpPost("{id}")]
    public async Task<IActionResult> Post(string id, [FromBody] VehicleDto dto)
    {
        var existing = _db.FindVehicle(dto.Registration);
        if (existing != default)
            return Conflict($"Sorry, there is already a vehicle with registration {dto.Registration} in the database.");
        var vehicleModel = _db.FindModel(dto.ModelCode);
        var vehicle = new Vehicle
        {
            Registration = dto.Registration,
            Color = dto.Color,
            Year = dto.Year,
            VehicleModel = vehicleModel
        };
        _db.CreateVehicle(vehicle);
        await PublishNewVehicleMessage(vehicle);
        return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
    }

    // DELETE api/vehicles/ABC123
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var vehicle = _db.FindVehicle(id);
        if (vehicle == default) return NotFound();
        _db.DeleteVehicle(vehicle);
        return NoContent();
    }
    
    private async Task PublishNewVehicleMessage(Vehicle vehicle) {
        var message = vehicle.ToMessage();
        await _bus.PubSub.PublishAsync(message);
    }
}