using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Transactions;

/// <summary>
/// Query para listar transações com paginação.
/// </summary>
public record GetTransactionsPagedQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<PagedResult<TransactionDto>>;
