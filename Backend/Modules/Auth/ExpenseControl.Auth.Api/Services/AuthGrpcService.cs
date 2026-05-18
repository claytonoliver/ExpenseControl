using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ExpenseControl.Auth.Application.Commands.Users;
using ExpenseControl.Auth.Application.Commands.Accounts;
using ExpenseControl.Shared.Grpc;
using MediatR;

namespace ExpenseControl.Auth.Api.Services;

public class AuthGrpcService : global::ExpenseControl.Shared.Grpc.AuthGrpcService.AuthGrpcServiceBase
{
    private readonly IMediator _mediator;

    public AuthGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<AuthTokenResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new RegisterUserCommand(request.Name, request.Email, request.Password),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToTokenResponse(result.Value!);
    }

    public override async Task<AuthTokenResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new LoginCommand(request.Email, request.Password),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.Unauthenticated, result.Error!));

        return ToTokenResponse(result.Value!);
    }

    public override async Task<InviteResponse> Invite(InviteRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new InviteUserCommand(
                Guid.Parse(request.AccountId),
                request.InvitedEmail,
                Guid.Parse(request.RequestingUserId)),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        var inv = result.Value!;
        return new InviteResponse
        {
            Id           = inv.Id.ToString(),
            AccountId    = inv.AccountId.ToString(),
            InvitedEmail = inv.InvitedEmail,
            ExpiresAt    = Timestamp.FromDateTime(inv.ExpiresAt.ToUniversalTime()),
            Status       = inv.Status
        };
    }

    public override async Task<AuthTokenResponse> AcceptInvitation(AcceptInvitationRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(
            new AcceptInvitationCommand(Guid.Parse(request.Token), Guid.Parse(request.UserId)),
            context.CancellationToken);

        if (result.IsFailure)
            throw new RpcException(new Status(StatusCode.InvalidArgument, result.Error!));

        return ToTokenResponse(result.Value!);
    }

    private static AuthTokenResponse ToTokenResponse(Application.DTOs.AuthTokenDto dto) => new()
    {
        Token     = dto.Token,
        UserId    = dto.UserId.ToString(),
        AccountId = dto.AccountId.ToString(),
        ExpiresAt = Timestamp.FromDateTime(dto.ExpiresAt.ToUniversalTime())
    };
}
