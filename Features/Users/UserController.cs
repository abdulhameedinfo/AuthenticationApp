using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/user")] // Base route for the controller
[Authorize]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserName()
    {
        return Ok(new { Name = "Hi my name is Abdul Hameed" });
    }
}