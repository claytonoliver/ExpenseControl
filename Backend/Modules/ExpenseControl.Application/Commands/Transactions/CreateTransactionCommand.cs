using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Comando para criar uma nova transação.
/// </summary>
public record CreateTransactionCommand(
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId,
    Guid PersonId) : IRequest<Result<TransactionDto>>;
