using ExpenseControl.Application.DTOs;
using MediatR;

namespace ExpenseControl.Application.Queries.Categories;

/// <summary>
/// Query para listar todas as categorias cadastradas.
/// </summary>
public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
