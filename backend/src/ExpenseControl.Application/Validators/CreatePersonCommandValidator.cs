using ExpenseControl.Application.Commands.Persons;
using FluentValidation;

namespace ExpenseControl.Application.Validators;

/// <summary>
/// Validador para comando de criação de pessoa.
/// </summary>
public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Age)
            .GreaterThan(0)
            .WithMessage("A idade deve ser um número inteiro positivo.");
    }
}
