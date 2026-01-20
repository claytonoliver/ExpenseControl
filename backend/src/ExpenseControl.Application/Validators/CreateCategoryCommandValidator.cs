using ExpenseControl.Application.Commands.Categories;
using ExpenseControl.Domain.Enums;
using FluentValidation;

namespace ExpenseControl.Application.Validators;

/// <summary>
/// Validador para comando de criação de categoria.
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.")
            .MaximumLength(200)
            .WithMessage("A descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Purpose)
            .IsInEnum()
            .WithMessage("A finalidade deve ser: Despesa (1), Receita (2) ou Ambas (3).");
    }
}
