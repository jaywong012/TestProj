using Application.Utilities;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class RegisterAccountCommandRequest : IRequest<bool>
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    public required string Role { get; init; }
}

public class RegisterAccountCommandRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterAccountCommandRequest, bool>
{
    public async Task<bool> Handle(RegisterAccountCommandRequest request, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword.Hash(request.Password);

        Account account = new()
        {
            UserName = request.UserName,
            Hash = hashedPassword,
            Role = request.Role
        };
        await unitOfWork.AccountRepository.Add(account);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}