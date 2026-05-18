using ExpenseControl.Api.Models.Requests;
using ExpenseControl.Application.Commands.Transactions;
using ExpenseControl.Application.Queries.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de transações.
/// Disponibiliza endpoints para criação e listagem de transações.
/// Aplica regras de negócio:
/// - Menores de idade só podem ter despesas.
/// - Categoria deve ser compatível com o tipo de transação.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todas as transações cadastradas.
    /// Inclui informações da pessoa e categoria associadas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lista transações com paginação.
    /// </summary>
    /// <param name="pageNumber">Número da página (1-based).</param>
    /// <param name="pageSize">Quantidade de itens por página (máx. 50).</param>
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetTransactionsPagedQuery(pageNumber, pageSize),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova transação.
    /// Valida:
    /// - Se a pessoa existe.
    /// - Se a categoria existe e é compatível com o tipo de transação.
    /// - Se a pessoa é menor de idade, apenas despesas são permitidas.
    /// </summary>
    /// <param name="request">Dados da transação a ser criada.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(
            request.Description,
            request.Value,
            request.Type,
            request.CategoryId,
            request.PersonId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Atualiza os dados de uma transação.
    /// A pessoa associada não pode ser alterada.
    /// </summary>
    /// <param name="id">Identificador da transação.</param>
    /// <param name="request">Dados atualizados da transação.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTransactionCommand(
            id,
            request.Description,
            request.Value,
            request.Type,
            request.CategoryId);

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
    /// Deleta uma transação.
    /// </summary>
    /// <param name="id">Identificador da transação.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTransactionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return NoContent();
    }
}
