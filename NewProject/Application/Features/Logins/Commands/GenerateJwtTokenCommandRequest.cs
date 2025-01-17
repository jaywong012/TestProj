using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    private readonly  IUnitOfWork _unitOfWork;
    private readonly string _key;
    public GenerateJwtTokenCommandRequestHandler(IUnitOfWork unitOfWork
        , IOptions<JwtSettings> jwtSettings
    )
    {
        _unitOfWork = unitOfWork;
        _key = jwtSettings.Value.Key;
    }

    public async Task<string> Handle(GenerateJwtTokenCommandRequest request, CancellationToken cancellationToken)
    {
        if (request.UserName != "admin" && request.Password != "123") return await Task.FromResult("");

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