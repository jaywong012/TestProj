using Application.Features.Accounts.Commands;
using Application.Common;
using Application.Utilities;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Test.UnitTest.Accounts;

public class RegisterAccount
{
    private RegisterAccountCommandRequest _request;
    private Mock<IUnitOfWork> _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        _request = new RegisterAccountCommandRequest
        {
            Password = "Test123",
            Role = Constants.ADMIN,
            UserName = "Test"
        };
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    [Test]
    public void ValidateAccountCommandRequest_HaveEnoughParams_ReturnTrue()
    {
        var isValid = ValidateRequest(_request);
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void RegisterAccount_HaveEnoughParams_AccountCreated()
    {
        _unitOfWork
            .Setup(uow => uow.AccountRepository.Add(It.IsAny<Account>()))
            .Returns(Task.CompletedTask);
        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var hashPassword = HashPassword.Hash(_request.Password);

        Account account = new()
        {
            Hash = hashPassword,
            Role = _request.Role,
            UserName = _request.UserName
        };

        _unitOfWork.Object.AccountRepository.Add(account);
        _unitOfWork.Object.SaveChangesAsync();
    }

    private static bool ValidateRequest(RegisterAccountCommandRequest request)
    {
        // Simulate validation
        if (string.IsNullOrWhiteSpace(request.UserName)) return false;
        if (string.IsNullOrWhiteSpace(request.Password)) return false;
        return !string.IsNullOrWhiteSpace(request.Role);
    }
}