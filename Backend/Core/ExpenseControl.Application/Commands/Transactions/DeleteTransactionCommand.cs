using ExpenseControl.Application.Common;
using MediatR;

namespace ExpenseControl.Application.Commands.Transactions;

/// <summary>
/// Comando para deletar uma transação.
/// </summary>
public record DeleteTransactionCommand(Guid Id) : IRequest<Result>;
