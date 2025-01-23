using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.Configurations.UnitTest;

namespace Test.UnitTest.Products.Queries;

public class GetProductList
{
    private List<Product> _mockProducts;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProducts = MockData.MockProducts();
    }

    [Test]
    public void GetAllProducts_EmptyListProducts_ReturnEmptyList()
    {
        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetAll())
            .Returns(new List<Product>().AsQueryable());

        var result = _mockUnitOfWork.Object.ProductRepository.GetAll();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllProducts_NotEmptyListProducts_Returns3Items()
    {
        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetAll())
            .Returns(_mockProducts.AsQueryable());

        var result = _mockUnitOfWork.Object.ProductRepository.GetAll();

        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetProductListByPaging_NotEmptyListProducts_ReturnExactPageAndSize()
    {
        const int size = 2;
        const int index = 1;
        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetAll())
            .Returns(_mockProducts.AsQueryable());

        var result = _mockUnitOfWork.Object.ProductRepository.GetAll();
        var paging = (int)Math.Ceiling(result.Count() / (decimal)size);
        var products = result
            .Skip(index * size)
            .Take(size)
            .ToList();

        Assert.Multiple(() =>
        {
            Assert.That(paging, Is.EqualTo(2));
            Assert.That(products, Has.Count.EqualTo(1));
        });
    }
}