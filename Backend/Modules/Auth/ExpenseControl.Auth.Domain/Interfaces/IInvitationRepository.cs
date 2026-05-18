using ExpenseControl.Auth.Domain.Entities;

namespace ExpenseControl.Auth.Domain.Interfaces;

public interface IInvitationRepository
{
    Task<Invitation?> GetByTokenAsync(Guid token, CancellationToken ct = default);
    Task              AddAsync(Invitation invitation, CancellationToken ct = default);
    Task              SaveChangesAsync(CancellationToken ct = default);
}
