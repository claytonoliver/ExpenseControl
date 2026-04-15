namespace ExpenseControl.Application.DTOs;

/// <summary>
/// DTO para retorno de dados de pessoa.
/// </summary>
public record PersonDto(
    Guid Id,
    string Name,
    int Age,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
