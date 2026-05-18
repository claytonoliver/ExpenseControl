using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Domain.Interfaces;
using ExpenseControl.Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Auth.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly AuthDbContext _ctx;

    public InvitationRepository(AuthDbContext ctx) => _ctx = ctx;

    public Task<Invitation?> GetByTokenAsync(Guid token, CancellationToken ct = default)
        => _ctx.Invitations.FirstOrDefaultAsync(i => i.Token == token, ct);

    public async Task AddAsync(Invitation invitation, CancellationToken ct = default)
        => await _ctx.Invitations.AddAsync(invitation, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
