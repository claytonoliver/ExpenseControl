using ExpenseControl.Application.DTOs;
using ExpenseControl.Application.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Persons;

/// <summary>
/// Handler para listagem de pessoas.
/// </summary>
public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDto>>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<PersonDto>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _personRepository.GetAllAsync(cancellationToken);

        return persons.Select(p => new PersonDto(
            p.Id,
            p.Name,
            p.Age,
            p.CreatedAt,
            p.UpdatedAt));
    }
}
