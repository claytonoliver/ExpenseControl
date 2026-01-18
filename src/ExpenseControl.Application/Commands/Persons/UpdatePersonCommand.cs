using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Comando para atualizar uma pessoa.
/// </summary>
public record UpdatePersonCommand(Guid Id, string Name, int Age) : IRequest<Result<PersonDto>>;
