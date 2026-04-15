using ExpenseControl.Application.Commands.Transactions;
using FluentValidation;

namespace ExpenseControl.Application.Validators;

/// <summary>
/// Validador para comando de criação de transação.
/// Valida dados básicos. Regras de negócio (menor de idade, compatibilidade categoria)
/// são validadas no handler com acesso ao banco.
/// </summary>
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.")
            .MaximumLength(500)
            .WithMessage("A descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("O valor deve ser um número positivo.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("O tipo deve ser: Despesa (1) ou Receita (2).");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("A pessoa é obrigatória.");
    }
}
