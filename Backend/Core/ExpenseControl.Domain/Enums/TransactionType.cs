namespace ExpenseControl.Domain.Enums;

/// <summary>
/// Tipo da transação financeira.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Despesa - saída de dinheiro.
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Receita - entrada de dinheiro.
    /// </summary>
    Income = 2
}
