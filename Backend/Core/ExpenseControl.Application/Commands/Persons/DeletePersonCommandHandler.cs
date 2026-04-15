using ExpenseControl.Application.Common;
using ExpenseControl.Application.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Handler para deleção de pessoa.
/// Remove a pessoa e todas as suas transações associadas.
/// </summary>
public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Result>
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DeletePersonCommandHandler(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        // Busca a pessoa
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);

        if (person is null)
            return Result.Failure("Pessoa não encontrada.");

        // Busca e remove todas as transações da pessoa
        var transactions = await _transactionRepository.GetByPersonIdAsync(request.Id, cancellationToken);

        if (transactions.Any())
            _transactionRepository.DeleteRange(transactions);

        // Remove a pessoa
        _personRepository.Delete(person);

        // Salva as alterações (usa o mesmo contexto, então é uma única transação)
        await _personRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
