using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using ExpenseControl.Auth.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Accounts;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<AuthTokenDto>>
{
    private readonly IInvitationRepository _invitations;
    private readonly IAccountRepository    _accounts;
    private readonly IUserRepository       _users;
    private readonly IJwtService           _jwt;

    public AcceptInvitationCommandHandler(
        IInvitationRepository invitations,
        IAccountRepository accounts,
        IUserRepository users,
        IJwtService jwt)
    {
        _invitations = invitations;
        _accounts    = accounts;
        _users       = users;
        _jwt         = jwt;
    }

    public async Task<Result<AuthTokenDto>> Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        var invitation = await _invitations.GetByTokenAsync(request.Token, ct);
        if (invitation is null || !invitation.IsValid())
            return Result.Failure<AuthTokenDto>("Convite inválido ou expirado.");

        var user = await _users.GetByIdAsync(request.UserId, ct);
        if (user is null)
            return Result.Failure<AuthTokenDto>("Usuário não encontrado.");

        var account = await _accounts.GetByIdAsync(invitation.AccountId, ct);
        if (account is null)
            return Result.Failure<AuthTokenDto>("Conta não encontrada.");

        try
        {
            account.Invite(user.Id);
            invitation.Accept();

            await _accounts.SaveChangesAsync(ct);
            await _invitations.SaveChangesAsync(ct);

            var expiresAt = DateTime.UtcNow.AddHours(8);
            var token     = _jwt.GenerateToken(user, account.Id);

            return Result.Success(new AuthTokenDto(token, user.Id, account.Id, expiresAt));
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<AuthTokenDto>(ex.Message);
        }
    }
}
