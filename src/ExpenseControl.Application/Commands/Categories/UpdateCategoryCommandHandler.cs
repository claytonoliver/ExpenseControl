using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Handler para atualização de categoria.
/// </summary>
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure<CategoryDto> ("Categoria não encontrada.");

        try
        {
            category.Update(request.Description, request.Purpose);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<CategoryDto>(ex.Message);
        }

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var purposeDescription = request.Purpose switch
        {
            CategoryPurpose.Expense => "Despesa",
            CategoryPurpose.Income => "Receita",
            CategoryPurpose.Both => "Ambas",
            _ => string.Empty
        };

        var dto = new CategoryDto(
            category.Id,
            category.Description,
            category.Purpose,
            purposeDescription,
            category.CreatedAt);

        return Result<CategoryDto>.Success(dto);
    }
}
