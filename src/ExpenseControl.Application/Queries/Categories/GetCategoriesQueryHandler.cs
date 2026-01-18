using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Queries.Categories;

/// <summary>
/// Handler para listagem de categorias.
/// </summary>
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return categories.Select(c => new CategoryDto(
            c.Id,
            c.Description,
            c.Purpose,
            GetPurposeDescription(c.Purpose),
            c.CreatedAt));
    }

    private static string GetPurposeDescription(CategoryPurpose purpose)
    {
        return purpose switch
        {
            CategoryPurpose.Expense => "Despesa",
            CategoryPurpose.Income => "Receita",
            CategoryPurpose.Both => "Ambas",
            _ => purpose.ToString()
        };
    }
}
