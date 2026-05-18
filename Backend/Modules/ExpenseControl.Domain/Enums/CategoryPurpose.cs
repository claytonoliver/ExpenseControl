namespace ExpenseControl.Domain.Enums;

/// <summary>
/// Finalidade da categoria.
/// Define para quais tipos de transação a categoria pode ser utilizada.
/// </summary>
public enum CategoryPurpose
{
    /// <summary>
    /// Categoria exclusiva para despesas.
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Categoria exclusiva para receitas.
    /// </summary>
    Income = 2,

    /// <summary>
    /// Categoria pode ser usada tanto para despesas quanto receitas.
    /// </summary>
    Both = 3
}
