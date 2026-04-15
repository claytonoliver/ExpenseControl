using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Application.Interfaces;

/// <summary>
/// Interface do repositório de pessoas.
/// </summary>
public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Person person, CancellationToken cancellationToken = default);
    void Delete(Person person);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
