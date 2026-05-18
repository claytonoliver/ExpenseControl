namespace ExpenseControl.Application.DTOs;

/// <summary>
/// DTO para retorno de totais por pessoa.
/// Contém o total de receitas, despesas e saldo.
/// </summary>
public record PersonTotalDto(
    Guid PersonId,
    string PersonName,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance);

/// <summary>
/// DTO para retorno da consulta de totais por pessoa.
/// Inclui lista de pessoas e total geral.
/// </summary>
public record PersonTotalsReportDto(
    IEnumerable<PersonTotalDto> PersonTotals,
    decimal GrandTotalIncome,
    decimal GrandTotalExpense,
    decimal GrandTotalBalance);
