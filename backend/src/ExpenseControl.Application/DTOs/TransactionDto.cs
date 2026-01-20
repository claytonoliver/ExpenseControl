using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Application.DTOs;

/// <summary>
/// DTO para retorno de dados de transação.
/// </summary>
public record TransactionDto(
    Guid Id,
    string Description,
    decimal Value,
    TransactionType Type,
    string TypeDescription,
    Guid CategoryId,
    string CategoryDescription,
    Guid PersonId,
    string PersonName,
    DateTime CreatedAt);
