using ExpenseControl.Bff.Models.Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using GrpcAuth       = ExpenseControl.Shared.Grpc;
using GrpcStatusCode = Grpc.Core.StatusCode;

namespace ExpenseControl.Bff.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly GrpcAuth.AuthGrpcService.AuthGrpcServiceClient _auth;

    public AuthController(GrpcAuth.AuthGrpcService.AuthGrpcServiceClient auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _auth.RegisterAsync(new GrpcAuth.RegisterRequest
            {
                Name     = request.Name,
                Email    = request.Email,
                Password = request.Password
            }, cancellationToken: ct);

            return Ok(MapToken(response));
        }
        catch (RpcException ex) when (ex.StatusCode == GrpcStatusCode.InvalidArgument)
        {
            return BadRequest(new { error = ex.Status.Detail });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _auth.LoginAsync(new GrpcAuth.LoginRequest
            {
                Email    = request.Email,
                Password = request.Password
            }, cancellationToken: ct);

            return Ok(MapToken(response));
        }
        catch (RpcException ex) when (ex.StatusCode == GrpcStatusCode.Unauthenticated)
        {
            return Unauthorized(new { error = ex.Status.Detail });
        }
    }

    [HttpPost("invite")]
    public async Task<IActionResult> Invite([FromBody] InviteRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _auth.InviteAsync(new GrpcAuth.InviteRequest
            {
                AccountId        = request.AccountId,
                InvitedEmail     = request.InvitedEmail,
                RequestingUserId = request.RequestingUserId
            }, cancellationToken: ct);

            return Ok(new
            {
                id           = response.Id,
                accountId    = response.AccountId,
                invitedEmail = response.InvitedEmail,
                expiresAt    = response.ExpiresAt.ToDateTime(),
                status       = response.Status
            });
        }
        catch (RpcException ex) when (ex.StatusCode == GrpcStatusCode.InvalidArgument)
        {
            return BadRequest(new { error = ex.Status.Detail });
        }
    }

    [HttpPost("accept-invitation")]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _auth.AcceptInvitationAsync(new GrpcAuth.AcceptInvitationRequest
            {
                Token  = request.Token,
                UserId = request.UserId
            }, cancellationToken: ct);

            return Ok(MapToken(response));
        }
        catch (RpcException ex) when (ex.StatusCode == GrpcStatusCode.InvalidArgument)
        {
            return BadRequest(new { error = ex.Status.Detail });
        }
    }

    private static object MapToken(GrpcAuth.AuthTokenResponse r) => new
    {
        token     = r.Token,
        userId    = r.UserId,
        accountId = r.AccountId,
        expiresAt = r.ExpiresAt.ToDateTime()
    };
}
