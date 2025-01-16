using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.UnitTest.Configurations;

namespace Test.UnitTest.Products;

public class UpdateProduct
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
        const string updateName = "Strong Bow";
        const decimal updatePrice = 100;
        var updateCategoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F431");

        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B");

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