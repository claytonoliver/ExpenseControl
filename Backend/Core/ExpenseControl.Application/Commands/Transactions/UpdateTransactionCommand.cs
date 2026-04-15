using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Comando para atualizar uma transação.
/// </summary>
public record UpdateTransactionCommand(
    Guid Id,
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId) : IRequest<Result<TransactionDto>>;
