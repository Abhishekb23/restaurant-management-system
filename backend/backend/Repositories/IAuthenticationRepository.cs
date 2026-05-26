namespace backend.Repositories;

using backend.Entities;


public interface IAuthenticationRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(
        string email);

    Task<ApplicationUser?> GetUserByPhoneAsync(
        string phone);

    Task CreateUserAsync(
        ApplicationUser user);

    Task CreateOtpAsync(
        OtpCode otp);

    Task<OtpCode?> GetOtpAsync(
        string destination,
        string code);

    Task MarkOtpUsedAsync(
        Guid otpId);
}