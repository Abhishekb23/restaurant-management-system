namespace backend.Repositories;

using backend.Entities;
using Dapper;

public class AuthenticationRepository
    : IAuthenticationRepository
{
    private readonly DapperContext _context;

    public AuthenticationRepository(
        DapperContext context)
    {
        _context = context;
    }

    // GET USER BY EMAIL
    public async Task<ApplicationUser?>
        GetUserByEmailAsync(
            string email)
    {
        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<ApplicationUser>(
                """
                SELECT *
                FROM "Users"
                WHERE "Email" = @Email
                LIMIT 1
                """,
                new
                {
                    Email = email
                });
    }

    // GET USER BY PHONE
    public async Task<ApplicationUser?>
        GetUserByPhoneAsync(
            string phone)
    {
        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<ApplicationUser>(
                """
                SELECT *
                FROM "Users"
                WHERE "PhoneNumber" = @Phone
                LIMIT 1
                """,
                new
                {
                    Phone = phone
                });
    }

    // CREATE USER
    public async Task CreateUserAsync(
        ApplicationUser user)
    {
        using var connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            INSERT INTO "Users"
            (
                "Id",
                "FullName",
                "Email",
                "UserName",
                "PasswordHash",
                "PhoneNumber",
                "ProfilePicture",
                "GoogleId",
                "Role",
                "CreatedAt"
            )
            VALUES
            (
                @Id,
                @FullName,
                @Email,
                @UserName,
                @PasswordHash,
                @PhoneNumber,
                @ProfilePicture,
                @GoogleId,
                @Role,
                @CreatedAt
            )
            """,
            user);
    }

    // CREATE OTP
    public async Task CreateOtpAsync(
        OtpCode otp)
    {
        using var connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            INSERT INTO "OtpCodes"
            (
                "Id",
                "Destination",
                "Code",
                "ExpiresAt",
                "IsUsed"
            )
            VALUES
            (
                @Id,
                @Destination,
                @Code,
                @ExpiresAt,
                @IsUsed
            )
            """,
            otp);
    }

    // GET OTP
    public async Task<OtpCode?>
        GetOtpAsync(
            string destination,
            string code)
    {
        using var connection =
            _context.CreateConnection();

        return await connection
            .QueryFirstOrDefaultAsync<OtpCode>(
                """
                SELECT *
                FROM "OtpCodes"
                WHERE "Destination" = @Destination
                AND "Code" = @Code
                AND "IsUsed" = false
                ORDER BY "ExpiresAt" DESC
                LIMIT 1
                """,
                new
                {
                    Destination = destination,
                    Code = code
                });
    }

    // MARK OTP USED
    public async Task MarkOtpUsedAsync(
        Guid otpId)
    {
        using var connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            UPDATE "OtpCodes"
            SET "IsUsed" = true
            WHERE "Id" = @Id
            """,
            new
            {
                Id = otpId
            });
    }

    // UPDATE GOOGLE LOGIN
    public async Task UpdateGoogleLoginAsync(
        Guid userId,
        string googleId,
        string? profilePicture)
    {
        using var connection =
            _context.CreateConnection();

        await connection.ExecuteAsync(
            """
            UPDATE "Users"
            SET
                "GoogleId" = @GoogleId,
                "ProfilePicture" = @ProfilePicture
            WHERE "Id" = @UserId
            """,
            new
            {
                UserId = userId,
                GoogleId = googleId,
                ProfilePicture = profilePicture
            });
    }

    // CHECK EMAIL EXISTS
    public async Task<bool>
        EmailExistsAsync(
            string email)
    {
        using var connection =
            _context.CreateConnection();

        var exists =
            await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM "Users"
                    WHERE "Email" = @Email
                )
                """,
                new
                {
                    Email = email
                });

        return exists;
    }

    // CHECK PHONE EXISTS
    public async Task<bool>
        PhoneExistsAsync(
            string phone)
    {
        using var connection =
            _context.CreateConnection();

        var exists =
            await connection.ExecuteScalarAsync<bool>(
                """
                SELECT EXISTS
                (
                    SELECT 1
                    FROM "Users"
                    WHERE "PhoneNumber" = @Phone
                )
                """,
                new
                {
                    Phone = phone
                });

        return exists;
    }
}