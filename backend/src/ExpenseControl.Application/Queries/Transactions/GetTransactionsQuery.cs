using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Transactions;

/// <summary>
/// Query para listar todas as transações cadastradas.
/// </summary>
public record GetTransactionsQuery : IRequest<IEnumerable<TransactionDto>>;
