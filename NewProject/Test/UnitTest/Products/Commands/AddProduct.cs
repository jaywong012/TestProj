using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.Configurations.UnitTest;

namespace Test.UnitTest.Products.Commands;

public class AddProduct
{
    private List<Product> _mockProducts;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProducts = MockData.MockProducts();
    }

    [Test]
    public async Task UpdateProduct_UpdateAllProductFields_AllFieldsUpdated()
    {
        const string addName = "Strong Bow";
        const decimal addPrice = 100;

        Product product = new()
        {
            Name = addName,
            Price = addPrice
        };

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.Add(It.IsAny<Product>()))
            .Callback<Product>(p => { _mockProducts.Add(p); })
            .Returns(Task.CompletedTask);

        await _mockUnitOfWork.Object.ProductRepository.Add(product);
    }
}