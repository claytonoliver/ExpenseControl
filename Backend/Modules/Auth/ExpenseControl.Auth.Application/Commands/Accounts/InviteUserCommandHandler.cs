using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Accounts;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Result<InvitationDto>>
{
    private readonly IAccountRepository    _accounts;
    private readonly IInvitationRepository _invitations;

    public InviteUserCommandHandler(IAccountRepository accounts, IInvitationRepository invitations)
    {
        _accounts    = accounts;
        _invitations = invitations;
    }

    public async Task<Result<InvitationDto>> Handle(InviteUserCommand request, CancellationToken ct)
    {
        var account = await _accounts.GetByIdAsync(request.AccountId, ct);
        if (account is null)
            return Result.Failure<InvitationDto>("Conta não encontrada.");

        if (account.OwnerId != request.RequestingUserId)
            return Result.Failure<InvitationDto>("Apenas o dono da conta pode convidar usuários.");

        try
        {
            var invitation = new Invitation(account.Id, request.InvitedEmail);
            await _invitations.AddAsync(invitation, ct);
            await _invitations.SaveChangesAsync(ct);

            return Result.Success(new InvitationDto(
                invitation.Id,
                invitation.AccountId,
                invitation.InvitedEmail,
                invitation.ExpiresAt,
                invitation.Status.ToString()));
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<InvitationDto>(ex.Message);
        }
    }
}
