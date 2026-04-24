using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Context;

namespace ExpenseControl.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpenseControlContext _context;

    public UnitOfWork(ExpenseControlContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
