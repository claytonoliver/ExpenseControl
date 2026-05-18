namespace ExpenseControl.Auth.Domain.Entities;

public enum InvitationStatus { Pending = 1, Accepted = 2, Revoked = 3 }

public class Invitation
{
    public Guid             Id           { get; private set; }
    public Guid             AccountId    { get; private set; }
    public string           InvitedEmail { get; private set; }
    public Guid             Token        { get; private set; }
    public DateTime         ExpiresAt    { get; private set; }
    public InvitationStatus Status       { get; private set; }
    public DateTime         CreatedAt    { get; private set; }

    private Invitation() { InvitedEmail = string.Empty; }

    public Invitation(Guid accountId, string invitedEmail, TimeSpan? validity = null)
    {
        if (string.IsNullOrWhiteSpace(invitedEmail))
            throw new ArgumentException("E-mail do convidado é obrigatório.");

        Id           = Guid.NewGuid();
        AccountId    = accountId;
        InvitedEmail = invitedEmail.Trim().ToLowerInvariant();
        Token        = Guid.NewGuid();
        ExpiresAt    = DateTime.UtcNow.Add(validity ?? TimeSpan.FromDays(7));
        Status       = InvitationStatus.Pending;
        CreatedAt    = DateTime.UtcNow;
    }

    public bool IsValid() => Status == InvitationStatus.Pending && ExpiresAt > DateTime.UtcNow;

    public void Accept()
    {
        if (!IsValid())
            throw new InvalidOperationException("Convite expirado ou já utilizado.");

        Status = InvitationStatus.Accepted;
    }

    public void Revoke() => Status = InvitationStatus.Revoked;
}
