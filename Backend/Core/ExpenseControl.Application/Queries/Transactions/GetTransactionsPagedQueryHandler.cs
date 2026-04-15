using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Queries.Transactions;

/// <summary>
/// Handler para listagem de transações com paginação.
/// </summary>
public class GetTransactionsPagedQueryHandler
    : IRequestHandler<GetTransactionsPagedQuery, PagedResult<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsPagedQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<PagedResult<TransactionDto>> Handle(
        GetTransactionsPagedQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = PaginationParams.Normalize(request.PageNumber, request.PageSize);

        var (transactions, totalCount) = await _transactionRepository.GetPagedAsync(
            pagination.PageNumber,
            pagination.PageSize,
            cancellationToken);

        var items = transactions.Select(t => new TransactionDto(
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

        return new PagedResult<TransactionDto>(
            items,
            pagination.PageNumber,
            pagination.PageSize,
            totalCount);
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
