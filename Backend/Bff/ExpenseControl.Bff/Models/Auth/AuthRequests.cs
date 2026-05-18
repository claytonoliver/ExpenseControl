namespace ExpenseControl.Bff.Models.Auth;

public record RegisterRequest(string Name, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record InviteRequest(string AccountId, string InvitedEmail, string RequestingUserId);

public record AcceptInvitationRequest(string Token, string UserId);
