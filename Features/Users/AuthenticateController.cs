using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")] // Base route for the controller
public class AuthController : ControllerBase
{
    private IJwtGenerator _jwtGenerator;
    public AuthController(IJwtGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        // Replace this with the real user validation logic (e.g, database check)
        if (loginRequest.Email == "test@example.com" && loginRequest.Password == "password")
        {
            var userId = Guid.NewGuid(); // Replace this with real user ID
            var token = _jwtGenerator.Generate(userId, loginRequest.Email);
            return Ok(new { Token = token });
        }

        return Unauthorized(new { Message = "Invalid credentials" });
    }
}