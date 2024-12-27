using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MinimoServer.Models;

namespace MinimoServer.Services;

public class JwtService(IConfiguration configuration)
{
    public string GenerateToken(Account account)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("AccountId", account.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"], // null
            audience: configuration["Jwt:Audience"], // null
            claims: claims,
            expires: DateTime.MaxValue,  
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}