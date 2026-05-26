using backend.Dtos;
using backend.Entities;
using backend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;


namespace backend.Services;

public class AuthenticationService
    : IAuthenticationService
{
    private readonly IAuthenticationRepository
        _repository;

    private readonly IConfiguration
        _configuration;

    public AuthenticationService(
        IAuthenticationRepository repository,
        IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(
        AuthenticationDto.RegisterDto dto)
    {
        var existing =
            await _repository
                .GetUserByEmailAsync(dto.Email);

        if (existing != null)
            return false;

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.Password),

            Role = "Customer",

            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateUserAsync(user);

        return true;
    }

    public async Task<string?> LoginAsync(
        AuthenticationDto.LoginDto dto)
    {
        var user =
            await _repository
                .GetUserByEmailAsync(dto.Email);

        if (user == null)
            return null;

        var valid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!valid)
            return null;

        return GenerateToken(user);
    }

    public async Task SendOtpAsync(
      AuthenticationDto.SendOtpDto dto)
    {
        var otp = new OtpCode
        {
            Id = Guid.NewGuid(),

            Destination = dto.Destination,

            Code =
                Random.Shared
                    .Next(100000, 999999)
                    .ToString(),

            ExpiresAt =
                DateTime.UtcNow.AddMinutes(5),

            IsUsed = false
        };

        await _repository.CreateOtpAsync(otp);

        var smtpHost =
            _configuration["Brevo:Host"];

        var smtpPort =
            int.Parse(
                _configuration["Brevo:Port"]!);

        // SMTP LOGIN
        var smtpEmail =
            _configuration["Brevo:Email"];

        var smtpPassword =
            _configuration["Brevo:Password"];

        // ACTUAL SENDER
        var senderEmail =
            _configuration["Brevo:SenderEmail"];

        var senderName =
            _configuration["Brevo:SenderName"];

        using var smtpClient =
            new SmtpClient(smtpHost)
            {
                Port = smtpPort,

                Credentials =
                    new NetworkCredential(
                        smtpEmail,
                        smtpPassword),

                EnableSsl = true,

                DeliveryMethod =
                    SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,

                Timeout = 10000
            };

        using var mail =
            new MailMessage
            {
                From =
                    new MailAddress(
                        senderEmail!,
                        senderName),

                Subject =
                    "Your OTP Verification Code",

                Body =
    $"""
<!DOCTYPE html>
<html>
<body style="
    font-family: Arial;
    background: #f5f7fb;
    padding: 30px;
">

<div style="
    max-width: 500px;
    margin: auto;
    background: white;
    border-radius: 16px;
    padding: 40px;
    box-shadow:
        0 10px 30px rgba(0,0,0,0.08);
">

<h1 style="
    color: #2563eb;
    margin-bottom: 20px;
">
Restaurant Management
</h1>

<p style="
    font-size: 16px;
    color: #444;
">
Your OTP verification code is:
</p>

<div style="
    background: #eff6ff;
    border-radius: 12px;
    padding: 20px;
    text-align: center;
    margin: 30px 0;
">

<h2 style="
    font-size: 42px;
    letter-spacing: 8px;
    color: #2563eb;
    margin: 0;
">
{otp.Code}
</h2>

</div>

<p style="
    color: #666;
    font-size: 14px;
">
This OTP will expire in 5 minutes.
</p>

<p style="
    color: #999;
    margin-top: 30px;
    font-size: 13px;
">
If you did not request this code,
please ignore this email.
</p>

</div>

</body>
</html>
""",

                IsBodyHtml = true
            };

        mail.To.Add(dto.Destination);

        try
        {
            Console.WriteLine(
                $"SENDING OTP TO: {dto.Destination}");

            Console.WriteLine(
                $"OTP CODE: {otp.Code}");

            await smtpClient.SendMailAsync(mail);

            Console.WriteLine(
                "EMAIL SENT SUCCESSFULLY");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "EMAIL FAILED");

            Console.WriteLine(
                ex.Message);

            Console.WriteLine(
                ex.InnerException?.Message);

            throw;
        }
    }

    public async Task<string?> VerifyOtpAsync(
        AuthenticationDto.VerifyOtpDto dto)
    {
        var otp =
            await _repository.GetOtpAsync(
                dto.Destination,
                dto.Code);

        if (otp == null)
            return null;

        if (otp.ExpiresAt < DateTime.UtcNow)
            return null;

        await _repository.MarkOtpUsedAsync(
            otp.Id);

        ApplicationUser? user;

        if (dto.Type == "EMAIL")
        {
            user =
                await _repository
                    .GetUserByEmailAsync(
                        dto.Destination);
        }
        else
        {
            user =
                await _repository
                    .GetUserByPhoneAsync(
                        dto.Destination);
        }

        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),

                UserName = dto.Destination,

                Email =
                    dto.Type == "EMAIL"
                        ? dto.Destination
                        : null,

                PhoneNumber =
                    dto.Type == "SMS"
                        ? dto.Destination
                        : null,

                Role = "Customer",

                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateUserAsync(user);
        }

        return GenerateToken(user);
    }

    public async Task<string?> GoogleLoginAsync(
        string email,
        string name,
        string? picture,
        string googleId)
    {
        var user =
            await _repository
                .GetUserByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),

                Email = email,

                UserName = email,

                FullName = name,

                ProfilePicture = picture,

                GoogleId = googleId,

                Role = "Customer",

                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateUserAsync(user);
        }

        return GenerateToken(user);
    }

    private string GenerateToken(
        ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email ?? ""),

            new(
                ClaimTypes.Name,
                user.FullName ?? ""),

            new(
                ClaimTypes.Role,
                user.Role ?? "Customer")
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

        var creds =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer:
                    _configuration["Jwt:Issuer"],

                audience:
                    _configuration["Jwt:Audience"],

                claims: claims,

                expires:
                    DateTime.UtcNow.AddDays(7),

                signingCredentials: creds);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}