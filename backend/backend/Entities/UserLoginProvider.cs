namespace backend.Entities;

public class UserLoginProvider
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Provider { get; set; } = default!;

    public string ProviderUserId { get; set; } = default!;

    public ApplicationUser User { get; set; } = default!;
}
