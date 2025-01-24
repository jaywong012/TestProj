using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Features.Accounts.Commands;
using Application.Common;
using Domain.Base;
using Domain.ErrorHandlingManagement;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Logins.Commands;

public class GenerateJwtTokenCommandRequest : IRequest<string>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}

public class GenerateJwtTokenCommandRequestHandler(
    IOptions<JwtSettings> jwtSettings,
    IMediator mediator) : IRequestHandler<GenerateJwtTokenCommandRequest, string>
{
    private readonly string _key = jwtSettings.Value.Key;

    public async Task<string> Handle(GenerateJwtTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var userQuery = new GetAccountByUserNameCommandRequest
        {
            UserName = request.UserName
        };
        var user = await mediator.Send(userQuery, cancellationToken);
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Hash))
        {
            throw new UnAuthorizeException("Invalid username or password.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodeKey = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, Constants.ADMIN)
            ]),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodeKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return await Task.FromResult(token);
    }
}