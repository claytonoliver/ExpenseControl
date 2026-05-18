using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ExpenseControl.Shared.Grpc;
using ExpenseControl.Application.Commands.Transactions;
using ExpenseControl.Application.Queries.Transactions;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Grpc.Services;

public class TransactionsGrpcService : TransactionGrpcService.TransactionGrpcServiceBase
{
    private readonly IMediator _mediator;

    public TransactionsGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<GetTransactionsResponse> GetAll(Empty request, ServerCallContext context)
    {
        var transactions = await _mediator.Send(new GetTransactionsQuery(), context.CancellationToken);

        var response = new GetTransactionsResponse();
        response.Transactions.AddRange(transactions.Select(ToResponse));
        return response;
    }

    public override async Task<TransactionResponse> Create(CreateTransactionRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new CreateTransactionCommand(
                request.Description,
                decimal.Parse(request.Value),
                (TransactionType)request.Type,
                Guid.Parse(request.CategoryId),
                Guid.Parse(request.PersonId)),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<TransactionResponse> Update(UpdateTransactionRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new UpdateTransactionCommand(
                Guid.Parse(request.Id),
                request.Description,
                decimal.Parse(request.Value),
                (TransactionType)request.Type,
                Guid.Parse(request.CategoryId)),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<DeleteTransactionResponse> Delete(DeleteTransactionRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new DeleteTransactionCommand(Guid.Parse(request.Id)),
            context.CancellationToken);

        return new DeleteTransactionResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess ? "Transação removida com sucesso." : result.Error!
        };
    }

    private static TransactionResponse ToResponse(Application.DTOs.TransactionDto t) => new()
    {
        Id                  = t.Id.ToString(),
        Description         = t.Description,
        Value               = t.Value.ToString("F2"),
        Type                = (int)t.Type,
        TypeDescription     = t.TypeDescription,
        CategoryId          = t.CategoryId.ToString(),
        CategoryDescription = t.CategoryDescription,
        PersonId            = t.PersonId.ToString(),
        PersonName          = t.PersonName,
        CreatedAt           = Timestamp.FromDateTime(t.CreatedAt.ToUniversalTime())
    };
}
