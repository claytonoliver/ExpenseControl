using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Handler para criação de categoria.
/// Valida os dados e persiste a nova categoria no banco.
/// </summary>
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Cria a entidade (validações no construtor)
            var category = new Category(request.Description, request.Purpose);

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            // Retorna o DTO com os dados da categoria criada
            var dto = new CategoryDto(
                category.Id,
                category.Description,
                category.Purpose,
                GetPurposeDescription(category.Purpose),
                category.CreatedAt);

            return Result.Success(dto);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<CategoryDto>(ex.Message);
        }
    }

    /// <summary>
    /// Converte o enum para descrição legível.
    /// </summary>
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
