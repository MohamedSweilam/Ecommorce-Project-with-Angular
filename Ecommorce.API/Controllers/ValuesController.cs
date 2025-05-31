using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProtectedController : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetProtectedData()
    {
        return Ok(new { message = "This is protected data." });
    }
}