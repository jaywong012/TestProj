using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.Configurations.UnitTest;

namespace Test.UnitTest.Products.Queries;

public class GetProductById
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
    public async Task GetProductById_EmptyList_ReturnEmptyObject()
    {
        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");
        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetById(productId).Result)
            .Returns(value: null);

        var result = await _mockUnitOfWork.Object.ProductRepository.GetById(productId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetProductById_FoundItem_ReturnCorrectItem()
    {
        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");
        var expectedProduct = _mockProducts.Find(p => p.Id == productId);

        Assert.That(expectedProduct, Is.Not.Null);

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetById(productId).Result)
            .Returns(expectedProduct);

        var result = await _mockUnitOfWork.Object.ProductRepository.GetById(productId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            if (result == null || expectedProduct == null) return;
            Assert.That(result.Id, Is.EqualTo(expectedProduct.Id));
            Assert.That(result.Price, Is.EqualTo(expectedProduct.Price));
            Assert.That(result.Name, Is.EqualTo(expectedProduct.Name));
        });
    }
}