using ExpenseControl.Application.Common;
using MediatR;

namespace ExpenseControl.Application.Commands.Categories;

/// <summary>
/// Comando para deletar uma categoria.
/// </summary>
public record DeleteCategoryCommand(Guid Id) : IRequest<Result>;
