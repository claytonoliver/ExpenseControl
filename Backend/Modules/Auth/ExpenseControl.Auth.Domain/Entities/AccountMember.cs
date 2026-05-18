namespace ExpenseControl.Auth.Domain.Entities;

public enum AccountRole { Owner = 1, Member = 2 }

public class AccountMember
{
    public Guid        AccountId { get; private set; }
    public Guid        UserId    { get; private set; }
    public AccountRole Role      { get; private set; }
    public DateTime    JoinedAt  { get; private set; }

    private AccountMember() { }

    internal AccountMember(Guid accountId, Guid userId, AccountRole role)
    {
        AccountId = accountId;
        UserId    = userId;
        Role      = role;
        JoinedAt  = DateTime.UtcNow;
    }
}
