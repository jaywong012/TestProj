using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.UnitTest.Configurations;

namespace Test.UnitTest.Categories;

public class UpdateCategory
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
    public async Task UpdateCategory_UpdateAllCategoryFields_AllFieldsUpdated()
    {
        const string updateName = "Strong Bow";

        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B");

        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetById(It.IsAny<Guid>()).Result)
            .Returns(_mockCategories.Find(p => p.Id == categoryId));

        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.Update(It.IsAny<Category>()))
            .Callback<Category>(category =>
            {
                var updateCategory = _mockCategories.Find(p => p.Id == category.Id);
                if (updateCategory == null) return;
                updateCategory.Name = updateName;
            })
            .Returns(Task.CompletedTask);

        var category = await _mockUnitOfWork.Object.CategoryRepository.GetById(categoryId);

        Assert.That(category, Is.Not.Null);
        if(category != null)
        {
            await _mockUnitOfWork.Object.CategoryRepository.Update(category);
            Assert.That(category.Name, Is.EqualTo(updateName));
        }
    }
}