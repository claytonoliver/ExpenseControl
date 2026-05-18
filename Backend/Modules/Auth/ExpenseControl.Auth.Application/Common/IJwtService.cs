using ExpenseControl.Auth.Domain.Entities;

namespace ExpenseControl.Auth.Application.Common;

public interface IJwtService
{
    string GenerateToken(User user, Guid activeAccountId);
}
