using ExpenseControl.Application.Common;
using ExpenseControl.Application.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Handler para deleção de categoria.
/// </summary>
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure("Categoria não encontrada.");

        // Verifica se existem transações associadas
        var hasTransactions = await _categoryRepository.HasTransactionsAsync(request.Id, cancellationToken);

        if (hasTransactions)
            return Result.Failure("Não é possível excluir a categoria pois existem transações associadas a ela.");

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
