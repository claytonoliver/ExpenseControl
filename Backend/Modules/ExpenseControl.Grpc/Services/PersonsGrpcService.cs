using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ExpenseControl.Shared.Grpc;
using ExpenseControl.Application.Commands.Persons;
using ExpenseControl.Application.Queries.Persons;
using MediatR;

namespace ExpenseControl.Grpc.Services;

public class PersonsGrpcService : PersonGrpcService.PersonGrpcServiceBase
{
    private readonly IMediator _mediator;

    public PersonsGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<GetPersonsResponse> GetAll(Empty request, ServerCallContext context)
    {
        var persons = await _mediator.Send(new GetPersonsQuery(), context.CancellationToken);

        var response = new GetPersonsResponse();
        response.Persons.AddRange(persons.Select(ToResponse));
        return response;
    }

    public override async Task<GetPersonTotalsResponse> GetTotals(Empty request, ServerCallContext context)
    {
        var report = await _mediator.Send(new GetPersonTotalsQuery(), context.CancellationToken);

        var response = new GetPersonTotalsResponse
        {
            GrandTotalIncome  = report.GrandTotalIncome.ToString("F2"),
            GrandTotalExpense = report.GrandTotalExpense.ToString("F2"),
            GrandTotalBalance = report.GrandTotalBalance.ToString("F2")
        };

        response.PersonTotals.AddRange(report.PersonTotals.Select(t => new PersonTotalResponse
        {
            PersonId     = t.PersonId.ToString(),
            PersonName   = t.PersonName,
            TotalIncome  = t.TotalIncome.ToString("F2"),
            TotalExpense = t.TotalExpense.ToString("F2"),
            Balance      = t.Balance.ToString("F2")
        }));

        return response;
    }

    public override async Task<PersonResponse> Create(CreatePersonRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new CreatePersonCommand(request.Name, request.Age),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<PersonResponse> Update(UpdatePersonRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new UpdatePersonCommand(Guid.Parse(request.Id), request.Name, request.Age),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<DeletePersonResponse> Delete(DeletePersonRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new DeletePersonCommand(Guid.Parse(request.Id)),
            context.CancellationToken);

        return new DeletePersonResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess ? "Pessoa removida com sucesso." : result.Error!
        };
    }

    private static PersonResponse ToResponse(Application.DTOs.PersonDto p) => new()
    {
        Id        = p.Id.ToString(),
        Name      = p.Name,
        Age       = p.Age,
        CreatedAt = Timestamp.FromDateTime(p.CreatedAt.ToUniversalTime()),
        UpdatedAt = p.UpdatedAt.HasValue
            ? Timestamp.FromDateTime(p.UpdatedAt.Value.ToUniversalTime())
            : null
    };
}
