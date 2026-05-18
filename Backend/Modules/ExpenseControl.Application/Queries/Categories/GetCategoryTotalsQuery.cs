using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Categories;

/// <summary>
/// Query para obter totais de receitas, despesas e saldo por categoria.
/// Inclui total geral ao final (opcional no requisito).
/// </summary>
public record GetCategoryTotalsQuery : IRequest<CategoryTotalsReportDto>;
