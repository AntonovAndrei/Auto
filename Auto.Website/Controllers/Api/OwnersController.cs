using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.Extensions;
using Auto.Website.Models;
using Castle.Core.Internal;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Auto.Website.Controllers.Api
{
    [Route("api/[controller]")]
	[ApiController]
	public class OwnersController : ControllerBase
	{
		private readonly IAutoDatabase db;
		private readonly IBus _bus;

		public OwnersController(IAutoDatabase db, IBus bus)
		{
			this.db = db;
			this._bus = bus;
		}

		// GET: api/owners
		[HttpGet]
		[Produces("application/hal+json")]
		public IActionResult Get(int index = 0, int count = 10)
		{
			var items = db.ListOwners().Skip(index).Take(count);
			var total = db.CountOwners();
			var _links = HypermediaExtensions.Paginate("/api/owners", index, count, total);
			var _actions = new
			{
				create = new
				{
					method = "POST",
					type = "application/json",
					name = "Create a new owner",
					href = "/api/owners"
				},
				delete = new
				{
					method = "DELETE",
					name = "Delete a owner",
					href = "/api/owners/{id}"
				}
			};
			var result = new
			{
				_links,
				_actions,
				index,
				count,
				total,
				items
			};
			return Ok(result);
		}

		// GET api/owners/ABC123
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var owner = db.FindOwner(id);
			if (owner == default) return NotFound();
			var json = owner.ToDynamic();
			json._links = new
			{
				self = new { href = $"/api/owners/{id}" },
				ownerVehicle = new { href = $"/api/vehicles/{owner.VehicleId}" }
			};
			json._actions = new
			{
				update = new
				{
					method = "PUT",
					href = $"/api/owners/{id}",
					accept = "application/json"
				},
				delete = new
				{
					method = "DELETE",
					href = $"/api/owners/{id}"
				}
			};
			return Ok(json);
		}

        // POST api/owners
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] OwnerDto dto)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);
			var ownerVehicle = db.FindVehicle(dto.VehicleId);
			var owner = new Owner
			{
				Id = dto.Id,
				FullName = dto.FullName,
				BirthDate = dto.BirthDate,
				VehicleId = dto.VehicleId,
				Vehicle = ownerVehicle,
			};

			db.CreateOwner(owner);
			

			return Ok(dto);
		}



		// PUT api/owners/ABC123
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] OwnerDto dto)
		{
			var ownerVehicle = db.FindVehicle(dto.VehicleId);
			var owner = new Owner
			{
				Id=id,
				FullName = dto.FullName,
				BirthDate = dto.BirthDate,
				VehicleId = dto.VehicleId,
				Vehicle = ownerVehicle,
			};
			db.UpdateOwner(owner);
			return Get(id);
		}

		// DELETE api/owners/ABC123
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var owner = db.FindOwner(id);
			if (owner == default) return NotFound();
			db.DeleteOwner(owner);
			return NoContent();
		}
		
		private async Task PublishOwnerMessage(Owner owner)
		{
			var message = owner.ToMessage();

			await _bus.PubSub.PublishAsync(message);
		}
	}
}
