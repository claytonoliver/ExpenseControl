using EasyNetQ;
using ExpenseControl.Application.Common;
using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Event.Persons;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Persons;

/// <summary>
/// Handler para criação de pessoa.
/// Valida os dados e persiste a nova pessoa no banco.
/// </summary>
public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Result<PersonDto>>
{
    private readonly IPersonRepository _personRepository;
    private readonly IBus _bus;

    public CreatePersonCommandHandler(IPersonRepository personRepository, IBus bus)
    {
        _personRepository = personRepository;
        _bus = bus;
    }

    public async Task<Result<PersonDto>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var person = new Person(request.Name, request.Age);

            await _personRepository.AddAsync(person, cancellationToken);
            await _personRepository.SaveChangesAsync(cancellationToken);

            var userCreted = await _personRepository.GetByIdAsync(person.Id, cancellationToken);

            if(userCreted == null)
                return Result.Failure<PersonDto>("Pessoa não encontrada após a criação.");
            
            var personCreatedEvent = new PersonCreatedEvent(userCreted.Id, userCreted.Name, userCreted.Age);
            await _bus.PubSub.PublishAsync(personCreatedEvent, cancellationToken);
            // Retorna o DTO com os dados da pessoa criada
            var dto = new PersonDto(
                userCreted.Id,
                userCreted.Name,
                userCreted.Age,
                userCreted.CreatedAt,
                userCreted.UpdatedAt);

            return Result.Success(dto);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PersonDto>(ex.Message);
        }
    }
}
