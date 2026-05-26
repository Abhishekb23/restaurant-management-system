namespace backend.Dtos;

public class AuthenticationDto
{


    public class VerifyOtpDto
    {
        public string Destination { get; set; } = default!;

        public string Code { get; set; } = default!;

        // EMAIL or SMS
        public string Type { get; set; } = default!;
    }

    public class SendOtpDto
    {
        public string Destination { get; set; } = default!;

        // EMAIL or SMS
        public string Type { get; set; } = default!;
    }

    public class LoginDto
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }

    public class RegisterDto
    {
        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
