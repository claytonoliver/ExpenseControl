namespace ExpenseControl.Auth.Domain.Entities;

public class Account
{
    public Guid     Id        { get; private set; }
    public Guid     OwnerId   { get; private set; }
    public string   Name      { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<AccountMember> Members => _members.AsReadOnly();
    private readonly List<AccountMember> _members = [];

    private Account() { Name = string.Empty; }

    public Account(Guid ownerId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da conta é obrigatório.");

        Id        = Guid.NewGuid();
        OwnerId   = ownerId;
        Name      = name.Trim();
        CreatedAt = DateTime.UtcNow;

        _members.Add(new AccountMember(Id, ownerId, AccountRole.Owner));
    }

    public AccountMember Invite(Guid userId)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new InvalidOperationException("Usuário já é membro desta conta.");

        var member = new AccountMember(Id, userId, AccountRole.Member);
        _members.Add(member);
        return member;
    }
}
