using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController
    : ControllerBase
{
    private readonly Services.IAuthenticationService
        _service;

    public AuthenticationController(
        Services.IAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        AuthenticationDto.RegisterDto dto)
    {
        var result =
            await _service.RegisterAsync(dto);

        if (!result)
            return BadRequest(
                "User already exists");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        AuthenticationDto.LoginDto dto)
    {
        var token =
            await _service.LoginAsync(dto);

        if (token == null)
            return Unauthorized();

        return Ok(new
        {
            token
        });
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(
        AuthenticationDto.SendOtpDto dto)
    {
        await _service.SendOtpAsync(dto);

        return Ok();
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(
        AuthenticationDto.VerifyOtpDto dto)
    {
        var token =
            await _service.VerifyOtpAsync(dto);

        if (token == null)
            return Unauthorized();

        return Ok(new
        {
            token
        });
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties =
            new AuthenticationProperties
            {
                RedirectUri = "https://localhost:7280/api/auth/google-response"
            };

        return Challenge(
            properties,
            GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result =
            await HttpContext.AuthenticateAsync(
                "Cookies");

        if (!result.Succeeded)
            return BadRequest(
                "Google authentication failed");

        var claims = result.Principal!.Claims;

        var email =
            claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.Email)?.Value;

        var name =
            claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.Name)?.Value;

        var googleId =
            claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.NameIdentifier)?.Value;

        var picture =
            claims.FirstOrDefault(x =>
                x.Type == "picture")?.Value;

        if (email == null || googleId == null)
            return BadRequest(
                "Invalid Google data");

        var token =
            await _service
                .GoogleLoginAsync(
                    email,
                    name ?? "",
                    picture,
                    googleId);

        return Redirect(
            $"http://localhost:4200/login?token={token}");
    }
}