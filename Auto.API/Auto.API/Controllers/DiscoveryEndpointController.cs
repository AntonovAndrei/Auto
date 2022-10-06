using Microsoft.AspNetCore.Mvc;

namespace Auto.API.Controllers;

[Route("api")]
[ApiController]
public class DiscoveryEndpointController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() {
        var welcome = new {
            _links = new {
                vehicles = new {
                    href = "/api/vehicles"
                }
            },
            message = "Welcome to the Auto API!",
        };
        return Ok(welcome);
    }
}