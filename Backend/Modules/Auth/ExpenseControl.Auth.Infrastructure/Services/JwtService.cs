using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseControl.Auth.Application.Common;
using ExpenseControl.Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseControl.Auth.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int    _expirationHours;

    public JwtService(IConfiguration config)
    {
        _secret          = config["Jwt:Secret"]          ?? throw new InvalidOperationException("Jwt:Secret não configurado.");
        _issuer          = config["Jwt:Issuer"]          ?? "ExpenseControl.Auth";
        _audience        = config["Jwt:Audience"]        ?? "ExpenseControl";
        _expirationHours = int.TryParse(config["Jwt:ExpirationHours"], out var h) ? h : 8;
    }

    public string GenerateToken(User user, Guid activeAccountId)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name,  user.Name),
            new Claim("accountId", activeAccountId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(_expirationHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
