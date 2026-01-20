using ExpenseControl.Application.Common;
using ExpenseControl.Application.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Handler para deleção de transação.
/// </summary>
public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction is null)
            return Result.Failure("Transação não encontrada.");

        _transactionRepository.Delete(transaction);
        await _transactionRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
