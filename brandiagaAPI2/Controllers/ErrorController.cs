using Microsoft.AspNetCore.Mvc;
using brandiagaAPI2.Dtos;

[ApiController]
[Route("api/[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Index([FromQuery] string message)
    {
        return Ok(ResponseDTO<object>.Error(message ?? "An error occurred. Please try again."));
    }
}