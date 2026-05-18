using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Domain.Interfaces;
using ExpenseControl.Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Auth.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AuthDbContext _ctx;

    public AccountRepository(AuthDbContext ctx) => _ctx = ctx;

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _ctx.Accounts.Include(a => a.Members).FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _ctx.Accounts
            .Include(a => a.Members)
            .Where(a => a.Members.Any(m => m.UserId == userId))
            .ToListAsync(ct);

    public async Task AddAsync(Account account, CancellationToken ct = default)
        => await _ctx.Accounts.AddAsync(account, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
