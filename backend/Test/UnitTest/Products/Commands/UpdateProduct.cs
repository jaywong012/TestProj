using Application.Features.Products.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.Configurations.UnitTest;

namespace Test.UnitTest.Products.Commands;

public class UpdateProduct
{
    private List<Product> _mockProducts;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private UpdateProductCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProducts = MockData.MockProducts();
        _request = new UpdateProductCommandRequest(
            "Strong Bow",
            100,
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F431"),
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"));
    }

    [Test]
    public async Task UpdateProduct_UpdateAllProductFields_AllFieldsUpdated()
    {
        var updateName = _request.Name;
        var updatePrice = _request.Price;
        var updateCategoryId = _request.CategoryId;

        var productId = _request.Id;

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetById(It.IsAny<Guid>()).Result)
            .Returns(_mockProducts.Find(p => p.Id == productId));

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.Update(It.IsAny<Product>()))
            .Callback<Product>(product =>
            {
                var updateProduct = _mockProducts.Find(p => p.Id == product.Id);
                if (updateProduct == null) return;
                updateProduct.Name = updateName;
                updateProduct.Price = updatePrice;
                updateProduct.CategoryId = updateCategoryId;
            })
            .Returns(Task.CompletedTask);

        var product = await _mockUnitOfWork.Object.ProductRepository.GetById(productId);

        Assert.That(product, Is.Not.Null);

        if (product != null)
        {
            await _mockUnitOfWork.Object.ProductRepository.Update(product);
            Assert.Multiple(() =>
            {
                Assert.That(product.Name, Is.EqualTo(updateName));
                Assert.That(product.Price, Is.EqualTo(updatePrice));
                Assert.That(product.CategoryId, Is.EqualTo(updateCategoryId));
            });
        }
    }
}