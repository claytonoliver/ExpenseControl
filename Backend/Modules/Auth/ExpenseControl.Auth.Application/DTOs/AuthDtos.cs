namespace ExpenseControl.Auth.Application.DTOs;

public record UserDto(Guid Id, string Name, string Email, DateTime CreatedAt);

public record AccountDto(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt);

public record AuthTokenDto(string Token, Guid UserId, Guid AccountId, DateTime ExpiresAt);

public record InvitationDto(Guid Id, Guid AccountId, string InvitedEmail, DateTime ExpiresAt, string Status);
