using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Application.Interfaces;

/// <summary>
/// Interface do repositório de transações.
/// </summary>
public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<Transaction> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    void Delete(Transaction transaction);
    void DeleteRange(IEnumerable<Transaction> transactions);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
