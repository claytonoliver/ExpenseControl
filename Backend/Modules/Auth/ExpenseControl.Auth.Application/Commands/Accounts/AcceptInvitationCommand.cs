using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Accounts;

public record AcceptInvitationCommand(Guid Token, Guid UserId) : IRequest<Result<AuthTokenDto>>;
