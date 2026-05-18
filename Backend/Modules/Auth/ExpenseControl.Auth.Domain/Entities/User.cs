namespace ExpenseControl.Auth.Domain.Entities;

public class User
{
    public Guid     Id           { get; private set; }
    public string   Name         { get; private set; }
    public string   Email        { get; private set; }
    public string   PasswordHash { get; private set; }
    public DateTime CreatedAt    { get; private set; }

    public IReadOnlyCollection<AccountMember> Memberships => _memberships.AsReadOnly();
    private readonly List<AccountMember> _memberships = [];

    private User() { Name = Email = PasswordHash = string.Empty; }

    public User(string name, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-mail é obrigatório.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Senha é obrigatória.");

        Id           = Guid.NewGuid();
        Name         = name.Trim();
        Email        = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        CreatedAt    = DateTime.UtcNow;
    }
}
