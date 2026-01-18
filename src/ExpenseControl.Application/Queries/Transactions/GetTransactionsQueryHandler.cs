using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Queries.Transactions;

/// <summary>
/// Handler para listagem de transações.
/// Inclui informações da categoria e pessoa associadas.
/// </summary>
public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllAsync(cancellationToken);

        return transactions.Select(t => new TransactionDto(
            t.Id,
            t.Description,
            t.Value,
            t.Type,
            GetTypeDescription(t.Type),
            t.CategoryId,
            t.Category?.Description ?? string.Empty,
            t.PersonId,
            t.Person?.Name ?? string.Empty,
            t.CreatedAt));
    }

    private static string GetTypeDescription(TransactionType type)
    {
        return type switch
        {
            TransactionType.Expense => "Despesa",
            TransactionType.Income => "Receita",
            _ => type.ToString()
        };
    }
}
