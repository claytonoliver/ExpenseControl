using ExpenseControl.Application.Common;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Comando para deletar uma pessoa e todas suas transações.
/// </summary>
public record DeletePersonCommand(Guid Id) : IRequest<Result>;
