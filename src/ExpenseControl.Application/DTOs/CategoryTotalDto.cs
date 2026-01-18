namespace ExpenseControl.Application.DTOs;

/// <summary>
/// DTO para retorno de totais por categoria.
/// Contém o total de receitas, despesas e saldo.
/// </summary>
public record CategoryTotalDto(
    Guid CategoryId,
    string CategoryDescription,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);

/// <summary>
/// DTO para retorno da consulta de totais por categoria.
/// Inclui lista de categorias e total geral.
/// </summary>
public record CategoryTotalsReportDto(
    IEnumerable<CategoryTotalDto> CategoryTotals,
    decimal GrandTotalIncome,
    decimal GrandTotalExpense,
    decimal GrandTotalBalance);
