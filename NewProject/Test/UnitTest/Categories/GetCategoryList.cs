using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.UnitTest.Configurations;

namespace Test.UnitTest.Categories;

public class GetCategoryList
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
    public void GetAllCategories_EmptyListCategory_ReturnEmptyList()
    {
        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetAll())
            .Returns(new List<Category>().AsQueryable());

        var result = _mockUnitOfWork.Object.CategoryRepository.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllCategories_NotEmptyListCategories_Returns3Items()
    {
        _mockUnitOfWork
            .Setup(uow => uow.CategoryRepository.GetAll())
            .Returns(_mockCategories.AsQueryable());

        var result = _mockUnitOfWork.Object.CategoryRepository.GetAll();

        Assert.That(result.Count(), Is.EqualTo(3));
    }
}