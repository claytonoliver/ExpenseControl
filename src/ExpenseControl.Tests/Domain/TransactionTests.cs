using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using FluentAssertions;

namespace ExpenseControl.Tests.Domain;

/// <summary>
/// Testes unitários para a entidade Transaction.
/// Valida regras de negócio como restrição de receita para menores.
/// </summary>
public class TransactionTests
{
    private readonly Person _adultPerson;
    private readonly Person _minorPerson;
    private readonly Category _expenseCategory;
    private readonly Category _incomeCategory;
    private readonly Category _bothCategory;

    public TransactionTests()
    {
        _adultPerson = new Person("Adulto", 25);
        _minorPerson = new Person("Menor", 16);
        _expenseCategory = new Category("Alimentação", CategoryPurpose.Expense);
        _incomeCategory = new Category("Salário", CategoryPurpose.Income);
        _bothCategory = new Category("Outros", CategoryPurpose.Both);
    }

    [Fact]
    public void Constructor_WithValidData_ShouldCreateTransaction()
    {
        // Arrange & Act
        var transaction = new Transaction(
            "Compra supermercado",
            150.50m,
            TransactionType.Expense,
            _expenseCategory.Id,
            _adultPerson.Id,
            _expenseCategory,
            _adultPerson);

        // Assert
        transaction.Description.Should().Be("Compra supermercado");
        transaction.Value.Should().Be(150.50m);
        transaction.Type.Should().Be(TransactionType.Expense);
        transaction.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_MinorWithIncome_ShouldThrowInvalidOperationException()
    {
        // Arrange & Act
        var act = () => new Transaction(
            "Mesada",
            100m,
            TransactionType.Income,
            _bothCategory.Id,
            _minorPerson.Id,
            _bothCategory,
            _minorPerson);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Menores de idade*só podem registrar despesas*");
    }

    [Fact]
    public void Constructor_MinorWithExpense_ShouldCreateTransaction()
    {
        // Arrange & Act
        var transaction = new Transaction(
            "Lanche",
            20m,
            TransactionType.Expense,
            _expenseCategory.Id,
            _minorPerson.Id,
            _expenseCategory,
            _minorPerson);

        // Assert
        transaction.Should().NotBeNull();
        transaction.Type.Should().Be(TransactionType.Expense);
    }

    [Fact]
    public void Constructor_IncompatibleCategory_ShouldThrowInvalidOperationException()
    {
        // Arrange - Tentar usar categoria de despesa para receita
        var act = () => new Transaction(
            "Salário",
            5000m,
            TransactionType.Income,
            _expenseCategory.Id,
            _adultPerson.Id,
            _expenseCategory,
            _adultPerson);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*categoria*não pode ser usada*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ShouldThrowArgumentException(string? invalidDescription)
    {
        // Act
        var act = () => new Transaction(
            invalidDescription!,
            100m,
            TransactionType.Expense,
            _expenseCategory.Id,
            _adultPerson.Id,
            _expenseCategory,
            _adultPerson);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*descrição*obrigatória*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Constructor_WithInvalidValue_ShouldThrowArgumentException(decimal invalidValue)
    {
        // Act
        var act = () => new Transaction(
            "Test",
            invalidValue,
            TransactionType.Expense,
            _expenseCategory.Id,
            _adultPerson.Id,
            _expenseCategory,
            _adultPerson);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*valor*positivo*");
    }

    [Theory]
    [InlineData(TransactionType.Expense, 100, -100)]
    [InlineData(TransactionType.Income, 100, 100)]
    public void GetSignedValue_ShouldReturnCorrectValue(
        TransactionType type,
        decimal value,
        decimal expectedSignedValue)
    {
        // Arrange
        var category = type == TransactionType.Expense ? _expenseCategory : _incomeCategory;
        var transaction = new Transaction(
            "Test",
            value,
            type,
            category.Id,
            _adultPerson.Id,
            category,
            _adultPerson);

        // Act
        var result = transaction.GetSignedValue();

        // Assert
        result.Should().Be(expectedSignedValue);
    }
}
