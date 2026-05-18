using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Users;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthTokenDto>>;
