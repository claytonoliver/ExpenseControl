using System.ComponentModel.DataAnnotations;
using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Api.Models.Requests;

/// <summary>
/// Request para criação de categoria.
/// </summary>
public record CreateCategoryRequest(
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    string Description,

    [Required(ErrorMessage = "A finalidade é obrigatória.")]
    [EnumDataType(typeof(CategoryPurpose), ErrorMessage = "A finalidade deve ser: Expense (1), Income (2) ou Both (3).")]
    CategoryPurpose Purpose);
