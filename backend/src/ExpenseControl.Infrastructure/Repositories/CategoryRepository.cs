using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de categorias usando Entity Framework.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseControlContext _context;

    public CategoryRepository(ExpenseControlContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .OrderBy(c => c.Description)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task<bool> HasTransactionsAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.AnyAsync(t => t.CategoryId == categoryId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
