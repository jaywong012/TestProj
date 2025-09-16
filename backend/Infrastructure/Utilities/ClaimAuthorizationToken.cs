using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities;

public static class ClaimAuthorizationToken
{
    public static UserProvider ClaimTokens(string jwtToken, string secretKey)
    {
        if (string.IsNullOrWhiteSpace(jwtToken))
            return null;
        if(jwtToken.Contains("Bearer ")) jwtToken = jwtToken.Replace("Bearer ", "").Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

            ValidateIssuer = false,    
            ValidateAudience = false, 
            ClockSkew = TimeSpan.Zero 
        };

        try
        {
            var principal = tokenHandler.ValidateToken(jwtToken, validationParams, out var validatedToken);

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var userName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new UserProvider
            {
                UserId = userId,
                UserName = userName,
                Role = role
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}