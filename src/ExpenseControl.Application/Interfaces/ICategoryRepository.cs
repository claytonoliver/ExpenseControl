using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Application.Interfaces;

/// <summary>
/// Interface do repositório de categorias.
/// </summary>
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    void Delete(Category category);
    Task<bool> HasTransactionsAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
