using backend.Dtos;
using backend.Entities;

namespace backend.Services;


public interface IAuthenticationService
{
    Task<bool> RegisterAsync(
        AuthenticationDto.RegisterDto dto);

    Task<string?> LoginAsync(
        AuthenticationDto.LoginDto dto);

    Task SendOtpAsync(
        AuthenticationDto.SendOtpDto dto);

    Task<string?> VerifyOtpAsync(
        AuthenticationDto.VerifyOtpDto dto);

    Task<string?> GoogleLoginAsync(
        string email,
        string name,
        string? picture,
        string googleId);
}
