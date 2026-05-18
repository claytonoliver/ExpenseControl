using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Entidade que representa uma categoria de transação.
/// Categorias definem a classificação das transações e sua finalidade (despesa, receita ou ambas).
/// </summary>
public class Category : Entity
{
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Finalidade da categoria: despesa, receita ou ambas.
    /// Define para quais tipos de transação esta categoria pode ser utilizada.
    /// </summary>
    public CategoryPurpose Purpose { get; private set; }

    /// <summary>
    /// Coleção de transações associadas a esta categoria.
    /// </summary>
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    // Construtor para EF Core
    private Category() => Description = null!;

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="description">Descrição da categoria (obrigatória).</param>
    /// <param name="purpose">Finalidade da categoria.</param>
    public Category(string description, CategoryPurpose purpose)
    {
        ValidateDescription(description);
        ValidatePurpose(purpose);

        Description = description;
        Purpose = purpose;
    }

    /// <summary>
    /// Atualiza os dados da categoria.
    /// </summary>
    public void Update(string description, CategoryPurpose purpose)
    {
        ValidateDescription(description);
        ValidatePurpose(purpose);

        Description = description;
        Purpose = purpose;
        SetUpdatedAt();
    }

    /// <summary>
    /// Verifica se a categoria pode ser usada para o tipo de transação especificado.
    /// </summary>
    /// <param name="transactionType">Tipo da transação a ser validada.</param>
    /// <returns>True se a categoria é compatível com o tipo de transação.</returns>
    public bool IsCompatibleWith(TransactionType transactionType)
    {
        return Purpose switch
        {
            CategoryPurpose.Both => true,
            CategoryPurpose.Expense => transactionType == TransactionType.Expense,
            CategoryPurpose.Income => transactionType == TransactionType.Income,
            _ => false
        };
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da categoria é obrigatória.", nameof(description));
    }

    private static void ValidatePurpose(CategoryPurpose purpose)
    {
        if (!Enum.IsDefined(typeof(CategoryPurpose), purpose))
            throw new ArgumentException("Finalidade da categoria inválida.", nameof(purpose));
    }
}
