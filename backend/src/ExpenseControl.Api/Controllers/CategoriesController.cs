using ExpenseControl.Api.Models.Requests;
using ExpenseControl.Application.Commands.Categories;
using ExpenseControl.Application.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de categorias.
/// Disponibiliza endpoints para criação e listagem de categorias.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todas as categorias cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtém totais de receitas, despesas e saldo por categoria.
    /// Inclui total geral ao final da listagem (opcional).
    /// </summary>
    [HttpGet("totals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotals(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryTotalsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="request">Dados da categoria a ser criada.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Description, request.Purpose);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Atualiza os dados de uma categoria.
    /// </summary>
    /// <param name="id">Identificador da categoria.</param>
    /// <param name="request">Dados atualizados da categoria.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(id, request.Description, request.Purpose);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error?.Contains("não encontrada") == true)
                return NotFound(new { error = result.Error });
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deleta uma categoria.
    /// Não é possível excluir categorias que possuem transações associadas.
    /// </summary>
    /// <param name="id">Identificador da categoria.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error?.Contains("não encontrada") == true)
                return NotFound(new { error = result.Error });
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }
}
