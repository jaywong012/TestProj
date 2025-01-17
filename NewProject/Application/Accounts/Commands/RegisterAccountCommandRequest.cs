using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Accounts.Commands;

public class RegisterAccountCommandRequest : IRequest<bool>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Role { get; set; }
}

public class RegisterAccountCommandRequestHandler : IRequestHandler<RegisterAccountCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public RegisterAccountCommandRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RegisterAccountCommandRequest request, CancellationToken cancellationToken)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        Account account = new()
        {
            UserName = request.UserName,
            Hash = hashedPassword,
            Salt = salt,
            Role = request.Role
        };
        await _unitOfWork.AccountRepository.Add(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}