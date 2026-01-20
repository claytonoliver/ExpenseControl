using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Entidade que representa uma transação financeira (despesa ou receita).
/// </summary>
public class Transaction : Entity
{
    /// <summary>
    /// Descrição da transação.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Valor da transação (deve ser positivo).
    /// </summary>
    public decimal Value { get; private set; }

    /// <summary>
    /// Tipo da transação: despesa ou receita.
    /// </summary>
    public TransactionType Type { get; private set; }

    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    public Guid CategoryId { get; private set; }

    /// <summary>
    /// Categoria da transação.
    /// </summary>
    public Category Category { get; private set; }

    /// <summary>
    /// Identificador da pessoa associada.
    /// </summary>
    public Guid PersonId { get; private set; }

    /// <summary>
    /// Pessoa responsável pela transação.
    /// </summary>
    public Person Person { get; private set; }

    // Construtor para EF Core
    private Transaction()
    {
        Description = null!;
        Category = null!;
        Person = null!;
    }

    /// <summary>
    /// Cria uma nova transação.
    /// </summary>
    /// <param name="description">Descrição da transação.</param>
    /// <param name="value">Valor da transação (positivo).</param>
    /// <param name="type">Tipo: despesa ou receita.</param>
    /// <param name="categoryId">ID da categoria.</param>
    /// <param name="personId">ID da pessoa.</param>
    /// <param name="category">Categoria para validação de compatibilidade.</param>
    /// <param name="person">Pessoa para validação de idade.</param>
    public Transaction(
        string description,
        decimal value,
        TransactionType type,
        Guid categoryId,
        Guid personId,
        Category category,
        Person person)
    {
        ValidateDescription(description);
        ValidateValue(value);
        ValidateType(type);
        ValidatePersonAge(person, type);
        ValidateCategoryCompatibility(category, type);

        Description = description;
        Value = value;
        Type = type;
        CategoryId = categoryId;
        PersonId = personId;
        Category = category;
        Person = person;
    }

    /// <summary>
    /// Atualiza os dados da transação.
    /// </summary>
    public void Update(
        string description,
        decimal value,
        TransactionType type,
        Guid categoryId,
        Category category)
    {
        ValidateDescription(description);
        ValidateValue(value);
        ValidateType(type);
        ValidateCategoryCompatibility(category, type);

        Description = description;
        Value = value;
        Type = type;
        CategoryId = categoryId;
        Category = category;
        SetUpdatedAt();
    }

    /// <summary>
    /// Retorna o valor com sinal: negativo para despesas, positivo para receitas.
    /// Útil para cálculos de saldo.
    /// </summary>
    public decimal GetSignedValue()
    {
        return Type == TransactionType.Expense ? -Value : Value;
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da transação é obrigatória.", nameof(description));
    }

    private static void ValidateValue(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("O valor da transação deve ser um número positivo.", nameof(value));
    }

    private static void ValidateType(TransactionType type)
    {
        if (!Enum.IsDefined(typeof(TransactionType), type))
            throw new ArgumentException("Tipo de transação inválido.", nameof(type));
    }

    /// <summary>
    /// Valida que menores de idade só podem ter despesas.
    /// </summary>
    private static void ValidatePersonAge(Person person, TransactionType type)
    {
        if (person.IsMinor() && type == TransactionType.Income)
            throw new InvalidOperationException("Menores de idade (menos de 18 anos) só podem registrar despesas.");
    }

    /// <summary>
    /// Valida que a categoria é compatível com o tipo de transação.
    /// </summary>
    private static void ValidateCategoryCompatibility(Category category, TransactionType type)
    {
        if (!category.IsCompatibleWith(type))
            throw new InvalidOperationException(
                $"A categoria '{category.Description}' não pode ser usada para transações do tipo {type}.");
    }
}
