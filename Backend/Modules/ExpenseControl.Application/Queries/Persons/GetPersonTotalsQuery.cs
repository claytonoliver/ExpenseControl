using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Persons;

/// <summary>
/// Query para obter totais de receitas, despesas e saldo por pessoa.
/// Inclui total geral ao final.
/// </summary>
public record GetPersonTotalsQuery : IRequest<PersonTotalsReportDto>;
