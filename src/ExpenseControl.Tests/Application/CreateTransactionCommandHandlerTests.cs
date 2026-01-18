using ExpenseControl.Application.Commands.Transactions;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using FluentAssertions;
using Moq;

namespace ExpenseControl.Tests.Application;

/// <summary>
/// Testes unitários para o handler de criação de transação.
/// Valida regras de negócio como menor de idade e compatibilidade de categoria.
/// </summary>
public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();

        _handler = new CreateTransactionCommandHandler(
            _transactionRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldReturnSuccessResult()
    {
        // Arrange
        var person = new Person("João", 25);
        var category = new Category("Alimentação", CategoryPurpose.Expense);

        var command = new CreateTransactionCommand(
            "Compra supermercado",
            150.50m,
            TransactionType.Expense,
            category.Id,
            person.Id);

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(person.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _transactionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _transactionRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Description.Should().Be("Compra supermercado");
        result.Value.Value.Should().Be(150.50m);
    }

    [Fact]
    public async Task Handle_WithNonExistentPerson_ShouldReturnFailure()
    {
        // Arrange
        var category = new Category("Alimentação", CategoryPurpose.Expense);

        var command = new CreateTransactionCommand(
            "Compra",
            100m,
            TransactionType.Expense,
            category.Id,
            Guid.NewGuid());

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Pessoa não encontrada");
    }

    [Fact]
    public async Task Handle_WithNonExistentCategory_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            "Compra",
            100m,
            TransactionType.Expense,
            Guid.NewGuid(),
            Guid.NewGuid());

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Categoria não encontrada");
    }

    [Fact]
    public async Task Handle_MinorWithIncome_ShouldReturnFailure()
    {
        // Arrange
        var minorPerson = new Person("Menor", 16);
        var category = new Category("Mesada", CategoryPurpose.Both);

        var command = new CreateTransactionCommand(
            "Mesada",
            100m,
            TransactionType.Income,
            category.Id,
            minorPerson.Id);

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(minorPerson.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(minorPerson);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Menores de idade");
    }

    [Fact]
    public async Task Handle_IncompatibleCategory_ShouldReturnFailure()
    {
        // Arrange
        var person = new Person("João", 25);
        var expenseCategory = new Category("Alimentação", CategoryPurpose.Expense);

        // Tentando usar categoria de despesa para receita
        var command = new CreateTransactionCommand(
            "Salário",
            5000m,
            TransactionType.Income,
            expenseCategory.Id,
            person.Id);

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(person.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(expenseCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expenseCategory);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("não pode ser usada");
    }
}
