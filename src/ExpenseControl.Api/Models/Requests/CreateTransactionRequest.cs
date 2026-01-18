using System.ComponentModel.DataAnnotations;
using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Api.Models.Requests;

/// <summary>
/// Request para criação de transação.
/// </summary>
public record CreateTransactionRequest(
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    string Description,

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo.")]
    decimal Value,

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [EnumDataType(typeof(TransactionType), ErrorMessage = "O tipo deve ser: Expense (1) ou Income (2).")]
    TransactionType Type,

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    Guid CategoryId,

    [Required(ErrorMessage = "A pessoa é obrigatória.")]
    Guid PersonId);
