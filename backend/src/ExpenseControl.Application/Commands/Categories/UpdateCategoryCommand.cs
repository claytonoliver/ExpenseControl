using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Comando para atualizar uma categoria.
/// </summary>
public record UpdateCategoryCommand(Guid Id, string Description, CategoryPurpose Purpose) : IRequest<Result<CategoryDto>>;
