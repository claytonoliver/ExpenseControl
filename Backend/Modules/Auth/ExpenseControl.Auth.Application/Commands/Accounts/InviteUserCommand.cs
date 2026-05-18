using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Accounts;

public record InviteUserCommand(Guid AccountId, string InvitedEmail, Guid RequestingUserId)
    : IRequest<Result<InvitationDto>>;
