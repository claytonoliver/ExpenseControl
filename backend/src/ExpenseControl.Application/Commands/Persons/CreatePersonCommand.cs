using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Comando para criar uma nova pessoa.
/// </summary>
public record CreatePersonCommand(string Name, int Age) : IRequest<Result<PersonDto>>;
