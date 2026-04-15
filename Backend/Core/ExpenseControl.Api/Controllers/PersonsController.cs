using ExpenseControl.Api.Models.Requests;
using ExpenseControl.Application.Commands.Persons;
using ExpenseControl.Application.Queries.Persons;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de pessoas.
/// Disponibiliza endpoints para criação, listagem e deleção de pessoas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPersonsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtém totais de receitas, despesas e saldo por pessoa.
    /// Inclui total geral ao final da listagem.
    /// </summary>
    [HttpGet("totals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotals(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPersonTotalsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    /// <param name="request">Dados da pessoa a ser criada.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePersonCommand(request.Name, request.Age);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa.
    /// </summary>
    /// <param name="id">Identificador da pessoa.</param>
    /// <param name="request">Dados atualizados da pessoa.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePersonCommand(id, request.Name, request.Age);
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
    /// Deleta uma pessoa e todas as suas transações associadas.
    /// </summary>
    /// <param name="id">Identificador da pessoa.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePersonCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return NoContent();
    }
}
