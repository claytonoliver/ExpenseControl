using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Handler para atualização de pessoa.
/// </summary>
public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Result<PersonDto>>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Result<PersonDto>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);

        if (person is null)
            return Result.Failure<PersonDto>("Pessoa não encontrada.");

        try
        {
            person.Update(request.Name, request.Age);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PersonDto>(ex.Message);
        }

        await _personRepository.SaveChangesAsync(cancellationToken);

        var dto = new PersonDto(
            person.Id,
            person.Name,
            person.Age,
            person.CreatedAt,
            person.UpdatedAt);

        return Result.Success(dto);
    }
}
