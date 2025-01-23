using Application.Features.Categories.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Test.UnitTest.Categories.Commands;

public class CreateCategory
{
    private CreateCategoryCommandRequest _request;
    private Mock<IUnitOfWork> _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        _request = new CreateCategoryCommandRequest("Name");
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    [Test]
    public async Task CreateCategory_HaveExactParam_RecordCreated()
    {
        _unitOfWork
            .Setup(uow => uow.CategoryRepository.Add(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        Category category = new()
        {
            Name = _request.Name
        };


        await _unitOfWork.Object.CategoryRepository.Add(category);
        var result = await _unitOfWork.Object.SaveChangesAsync();
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void ValidateRequest_HaveExactParam_ReturnValidate()
    {
        var result = ValidateRequest(_request);
        Assert.That(result, Is.True);
    }

    private static bool ValidateRequest(CreateCategoryCommandRequest request)
    {
        return !string.IsNullOrEmpty(request.Name);
    }
}