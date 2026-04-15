using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Persons;

/// <summary>
/// Query para listar todas as pessoas cadastradas.
/// </summary>
public record GetPersonsQuery : IRequest<IEnumerable<PersonDto>>;
