using ExpenseControl.Application.Commands.Persons;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ExpenseControl.Tests.Application;

/// <summary>
/// Testes unitários para o handler de criação de pessoa.
/// </summary>
public class CreatePersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly CreatePersonCommandHandler _handler;

    public CreatePersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new CreatePersonCommandHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldReturnSuccessResult()
    {
        // Arrange
        var command = new CreatePersonCommand("João Silva", 25);

        _personRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _personRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("João Silva");
        result.Value.Age.Should().Be(25);

        _personRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _personRepositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidName_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreatePersonCommand("", 25);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("nome");

        _personRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidAge_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new CreatePersonCommand("João", 0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("idade");

        _personRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
