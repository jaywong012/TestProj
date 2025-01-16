using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.UnitTest.Configurations;

namespace Test.UnitTest.Categories;

public class GetCategoryById
{
    private List<Category> _mockCategories;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCategories = MockData.MockCategories();
    }

    [Test]
    public async Task GetCategoryById_EmptyList_ReturnEmptyObject()
    {
        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");
        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetById(categoryId).Result)
            .Returns(value: null);

        var result = await _mockUnitOfWork.Object.CategoryRepository.GetById(categoryId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetCategoryById_FoundItem_ReturnCorrectItem()
    {
        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");
        var expectedCategory = _mockCategories.Find(p => p.Id == categoryId);

        Assert.That(expectedCategory, Is.Not.Null);

        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetById(categoryId).Result)
            .Returns(expectedCategory);

        var result = await _mockUnitOfWork.Object.CategoryRepository.GetById(categoryId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            if(result == null || expectedCategory == null) return;
            Assert.That(result.Id, Is.EqualTo(expectedCategory.Id));
            Assert.That(result.Name, Is.EqualTo(expectedCategory.Name));
        });
    }
}