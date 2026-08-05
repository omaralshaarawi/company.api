// Controllers/AuthController.cs
using Azure.Core;
using company.api.Data;
using company.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly CompanyContext _db;
    private readonly TokenService _tokenService;
    public AuthController(CompanyContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }
    public record LoginRequest(string Username, string Password);
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid username or password.");
        var token = _tokenService.CreateToken(user);
        return Ok(new { token });
    }
}