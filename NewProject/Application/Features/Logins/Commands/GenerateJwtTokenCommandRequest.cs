using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Features.Accounts.Commands;
using Application.Common;
using Domain.Base;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Logins.Commands;

public class GenerateJwtTokenCommandRequest : IRequest<string>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
}

public class GenerateJwtTokenCommandRequestHandler : IRequestHandler<GenerateJwtTokenCommandRequest, string>
{
    private readonly string _key;
    private readonly IMediator _mediator;
    public GenerateJwtTokenCommandRequestHandler(IUnitOfWork unitOfWork
        , IOptions<JwtSettings> jwtSettings,
        IMediator mediator
    )
    {
        _key = jwtSettings.Value.Key;
        _mediator = mediator;
    }

    public async Task<string> Handle(GenerateJwtTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var userQuery = new GetAccountByUserNameCommandRequest
        {
            UserName = request.UserName
        };
        var user = await _mediator.Send(userQuery, cancellationToken);
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Hash))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodeKey = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, Constants.ADMIN),
            }),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodeKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return await Task.FromResult(token);
    }
}