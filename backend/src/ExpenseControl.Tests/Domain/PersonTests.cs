using ExpenseControl.Domain.Entities;
using FluentAssertions;

namespace ExpenseControl.Tests.Domain;

/// <summary>
/// Testes unitários para a entidade Person.
/// </summary>
public class PersonTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePerson()
    {
        // Arrange
        var name = "João Silva";
        var age = 25;

        // Act
        var person = new Person(name, age);

        // Assert
        person.Name.Should().Be(name);
        person.Age.Should().Be(age);
        person.Id.Should().NotBeEmpty();
        person.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
    {
        // Act
        var act = () => new Person(invalidName!, 25);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*nome*obrigatório*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithInvalidAge_ShouldThrowArgumentException(int invalidAge)
    {
        // Act
        var act = () => new Person("João", invalidAge);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*idade*positivo*");
    }

    [Theory]
    [InlineData(17, true)]
    [InlineData(18, false)]
    [InlineData(1, true)]
    [InlineData(65, false)]
    public void IsMinor_ShouldReturnCorrectValue(int age, bool expectedIsMinor)
    {
        // Arrange
        var person = new Person("Test", age);

        // Act
        var result = person.IsMinor();

        // Assert
        result.Should().Be(expectedIsMinor);
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdatePersonAndSetUpdatedAt()
    {
        // Arrange
        var person = new Person("João", 25);
        var newName = "João Silva";
        var newAge = 26;

        // Act
        person.Update(newName, newAge);

        // Assert
        person.Name.Should().Be(newName);
        person.Age.Should().Be(newAge);
        person.UpdatedAt.Should().NotBeNull();
        person.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
