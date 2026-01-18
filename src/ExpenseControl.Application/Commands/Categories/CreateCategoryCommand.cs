using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Comando para criar uma nova categoria.
/// </summary>
public record CreateCategoryCommand(string Description, CategoryPurpose Purpose) : IRequest<Result<CategoryDto>>;
