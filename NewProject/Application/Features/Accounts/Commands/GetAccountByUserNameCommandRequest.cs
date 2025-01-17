using Domain.Entities;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class GetAccountByUserNameCommandRequest : IRequest<Account>
{
    public required string UserName { get; set; }
}

public class GetAccountByUserNameCommandRequestHandler : IRequestHandler<GetAccountByUserNameCommandRequest, Account>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAccountByUserNameCommandRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<Account> Handle(GetAccountByUserNameCommandRequest request, CancellationToken cancellationToken)
    {
        var account = _unitOfWork.AccountRepository
            .GetAll()
            .FirstOrDefault(a => a.UserName == request.UserName);
        return account == null ? throw new ItemNotFoundException("not found account") : Task.FromResult(account);
    }
}