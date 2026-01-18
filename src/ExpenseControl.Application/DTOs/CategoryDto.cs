using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Application.DTOs;

/// <summary>
/// DTO para retorno de dados de categoria.
/// </summary>
public record CategoryDto(
    Guid Id,
    string Description,
    CategoryPurpose Purpose,
    string PurposeDescription,
    DateTime CreatedAt);
