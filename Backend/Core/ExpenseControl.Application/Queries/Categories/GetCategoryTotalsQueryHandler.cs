using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Queries.Categories;

/// <summary>
/// Handler para consulta de totais por categoria.
/// Calcula total de receitas, despesas e saldo para cada categoria,
/// além do total geral.
/// </summary>
public class GetCategoryTotalsQueryHandler : IRequestHandler<GetCategoryTotalsQuery, CategoryTotalsReportDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetCategoryTotalsQueryHandler(
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository)
    {
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<CategoryTotalsReportDto> Handle(GetCategoryTotalsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        var allTransactions = await _transactionRepository.GetAllAsync(cancellationToken);

        var categoryTotals = new List<CategoryTotalDto>();

        // Calcula totais para cada categoria
        foreach (var category in categories)
        {
            var categoryTransactions = allTransactions.Where(t => t.CategoryId == category.Id);

            var totalIncome = categoryTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value);

            var totalExpense = categoryTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value);

            var balance = totalIncome - totalExpense;

            categoryTotals.Add(new CategoryTotalDto(
                category.Id,
                category.Description,
                totalIncome,
                totalExpense,
                balance));
        }

        // Calcula totais gerais
        var grandTotalIncome = categoryTotals.Sum(c => c.TotalIncome);
        var grandTotalExpense = categoryTotals.Sum(c => c.TotalExpense);
        var grandTotalBalance = grandTotalIncome - grandTotalExpense;

        return new CategoryTotalsReportDto(
            categoryTotals,
            grandTotalIncome,
            grandTotalExpense,
            grandTotalBalance);
    }
}
