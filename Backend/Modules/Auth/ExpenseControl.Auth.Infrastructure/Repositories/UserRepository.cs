using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Domain.Interfaces;
using ExpenseControl.Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _ctx;

    public UserRepository(AuthDbContext ctx) => _ctx = ctx;

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _ctx.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _ctx.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await _ctx.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
