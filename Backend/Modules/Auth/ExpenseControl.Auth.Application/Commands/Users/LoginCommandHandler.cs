using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using ExpenseControl.Auth.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Users;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthTokenDto>>
{
    private readonly IUserRepository    _users;
    private readonly IAccountRepository _accounts;
    private readonly IJwtService        _jwt;
    private readonly IPasswordHasher    _hasher;

    public LoginCommandHandler(IUserRepository users, IAccountRepository accounts, IJwtService jwt, IPasswordHasher hasher)
    {
        _users    = users;
        _accounts = accounts;
        _jwt      = jwt;
        _hasher   = hasher;
    }

    public async Task<Result<AuthTokenDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<AuthTokenDto>("E-mail ou senha inválidos.");

        var accounts = await _accounts.GetByUserIdAsync(user.Id, ct);
        var account = accounts.FirstOrDefault(a => a.OwnerId == user.Id)
                      ?? accounts.FirstOrDefault();

        if (account is null)
            return Result.Failure<AuthTokenDto>("Nenhuma conta encontrada para este usuário.");

        var expiresAt = DateTime.UtcNow.AddHours(8);
        var token     = _jwt.GenerateToken(user, account.Id);

        return Result.Success(new AuthTokenDto(token, user.Id, account.Id, expiresAt));
    }
}
