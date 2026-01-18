using System.ComponentModel.DataAnnotations;

namespace ExpenseControl.Api.Models.Requests;

/// <summary>
/// Request para atualização de pessoa.
/// </summary>
public record UpdatePersonRequest(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
    string Name,

    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A idade deve ser um número inteiro positivo.")]
    int Age);
