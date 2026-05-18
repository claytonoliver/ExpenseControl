using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using FluentAssertions;

namespace ExpenseControl.Tests.Domain;

/// <summary>
/// Testes unitários para a entidade Category.
/// </summary>
public class CategoryTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCategory()
    {
        // Arrange
        var description = "Alimentação";
        var purpose = CategoryPurpose.Expense;

        // Act
        var category = new Category(description, purpose);

        // Assert
        category.Description.Should().Be(description);
        category.Purpose.Should().Be(purpose);
        category.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ShouldThrowArgumentException(string? invalidDescription)
    {
        // Act
        var act = () => new Category(invalidDescription!, CategoryPurpose.Expense);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*descrição*obrigatória*");
    }

    [Theory]
    [InlineData(CategoryPurpose.Expense, TransactionType.Expense, true)]
    [InlineData(CategoryPurpose.Expense, TransactionType.Income, false)]
    [InlineData(CategoryPurpose.Income, TransactionType.Income, true)]
    [InlineData(CategoryPurpose.Income, TransactionType.Expense, false)]
    [InlineData(CategoryPurpose.Both, TransactionType.Expense, true)]
    [InlineData(CategoryPurpose.Both, TransactionType.Income, true)]
    public void IsCompatibleWith_ShouldReturnCorrectValue(
        CategoryPurpose purpose,
        TransactionType transactionType,
        bool expectedResult)
    {
        // Arrange
        var category = new Category("Test", purpose);

        // Act
        var result = category.IsCompatibleWith(transactionType);

        // Assert
        result.Should().Be(expectedResult);
    }
}
