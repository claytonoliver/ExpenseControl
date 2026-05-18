using ExpenseControl.Auth.Domain.Entities;

namespace ExpenseControl.Auth.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?>             GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task                       AddAsync(Account account, CancellationToken ct = default);
    Task                       SaveChangesAsync(CancellationToken ct = default);
}
