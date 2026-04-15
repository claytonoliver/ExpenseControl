using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Handler para criação de pessoa.
/// Valida os dados e persiste a nova pessoa no banco.
/// </summary>
public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Result<PersonDto>>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Result<PersonDto>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Cria a entidade (validações no construtor)
            var person = new Person(request.Name, request.Age);

            await _personRepository.AddAsync(person, cancellationToken);
            await _personRepository.SaveChangesAsync(cancellationToken);

            // Retorna o DTO com os dados da pessoa criada
            var dto = new PersonDto(
                person.Id,
                person.Name,
                person.Age,
                person.CreatedAt,
                person.UpdatedAt);

            return Result.Success(dto);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PersonDto>(ex.Message);
        }
    }
}
