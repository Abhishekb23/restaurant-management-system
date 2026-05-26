namespace backend.Entities;

public class OtpCode
{
    public Guid Id { get; set; }

    public string Destination { get; set; } = default!;

    public string Code { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public string Purpose { get; set; } = "LOGIN";
}