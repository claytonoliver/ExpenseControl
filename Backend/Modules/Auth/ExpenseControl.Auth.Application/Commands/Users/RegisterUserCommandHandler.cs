using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Application.DTOs;
using ExpenseControl.Auth.Domain.Entities;
using ExpenseControl.Auth.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Auth.Application.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthTokenDto>>
{
    private readonly IUserRepository    _users;
    private readonly IAccountRepository _accounts;
    private readonly IJwtService        _jwt;
    private readonly IPasswordHasher    _hasher;

    public RegisterUserCommandHandler(IUserRepository users, IAccountRepository accounts, IJwtService jwt, IPasswordHasher hasher)
    {
        _users    = users;
        _accounts = accounts;
        _jwt      = jwt;
        _hasher   = hasher;
    }

    public async Task<Result<AuthTokenDto>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        try
        {
            var existing = await _users.GetByEmailAsync(request.Email, ct);
            if (existing is not null)
                return Result.Failure<AuthTokenDto>("E-mail já cadastrado.");

            var hash    = _hasher.Hash(request.Password);
            var user    = new User(request.Name, request.Email, hash);
            var account = new Account(user.Id, $"Conta de {user.Name}");

            await _users.AddAsync(user, ct);
            await _users.SaveChangesAsync(ct);

            await _accounts.AddAsync(account, ct);
            await _accounts.SaveChangesAsync(ct);

            var expiresAt = DateTime.UtcNow.AddHours(8);
            var token     = _jwt.GenerateToken(user, account.Id);

            return Result.Success(new AuthTokenDto(token, user.Id, account.Id, expiresAt));
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<AuthTokenDto>(ex.Message);
        }
    }
}
