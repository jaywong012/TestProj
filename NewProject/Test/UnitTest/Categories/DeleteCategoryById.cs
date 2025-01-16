using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.UnitTest.Configurations;

namespace Test.UnitTest.Categories;

public class DeleteCategoryById
{
    private List<Category> _mockCategories;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCategories = MockData.MockCategories();
    }

    [Test]
    public async Task DeleteCategoryById_CategoryExists_CategoryIsDeleted()
    {
        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B");

        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetById(It.IsAny<Guid>()).Result)
            .Returns<Guid>(id => _mockCategories.Find(p => p.Id == id && !p.IsDeleted));

        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.Delete(It.IsAny<Guid>()))
            .Callback<Guid>(id =>
            {
                var category = _mockCategories.Find(p => p.Id == id);
                if (category != null)
                {
                    category.IsDeleted = true;
                }
            })
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var categoryPrevState = await _mockUnitOfWork.Object.CategoryRepository.GetById(categoryId);
        if(categoryPrevState != null)
        {
            Assert.That(categoryPrevState, Is.Not.Null);
            Assert.That(categoryPrevState.IsDeleted, Is.False);
        }

        await _mockUnitOfWork.Object.CategoryRepository.Delete(categoryId);
        await _mockUnitOfWork.Object.SaveChangesAsync();

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        var category = await _mockUnitOfWork.Object.CategoryRepository.GetById(categoryId);
        Assert.That(category, Is.Null);
    }
}