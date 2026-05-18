using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ExpenseControl.Shared.Grpc;
using ExpenseControl.Application.Commands.Categories;
using ExpenseControl.Application.Queries.Categories;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Grpc.Services;

public class CategoriesGrpcService : CategoryGrpcService.CategoryGrpcServiceBase
{
    private readonly IMediator _mediator;

    public CategoriesGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<GetCategoriesResponse> GetAll(Empty request, ServerCallContext context)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), context.CancellationToken);

        var response = new GetCategoriesResponse();
        response.Categories.AddRange(categories.Select(ToResponse));
        return response;
    }

    public override async Task<CategoryResponse> Create(CreateCategoryRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new CreateCategoryCommand(request.Description, (CategoryPurpose)request.Purpose),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<CategoryResponse> Update(UpdateCategoryRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new UpdateCategoryCommand(Guid.Parse(request.Id), request.Description, (CategoryPurpose)request.Purpose),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToResponse(result.Value!);
    }

    public override async Task<DeleteCategoryResponse> Delete(DeleteCategoryRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new DeleteCategoryCommand(Guid.Parse(request.Id)),
            context.CancellationToken);

        return new DeleteCategoryResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess ? "Categoria removida com sucesso." : result.Error!
        };
    }

    private static CategoryResponse ToResponse(Application.DTOs.CategoryDto c) => new()
    {
        Id                 = c.Id.ToString(),
        Description        = c.Description,
        Purpose            = (int)c.Purpose,
        PurposeDescription = c.PurposeDescription,
        CreatedAt          = Timestamp.FromDateTime(c.CreatedAt.ToUniversalTime())
    };
}
