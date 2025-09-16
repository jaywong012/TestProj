using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Test.Configurations.UnitTest;

namespace Test.UnitTest.Products.Commands;

public class DeleteProductById
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
    public async Task DeleteProductById_ProductExists_ProductIsDeleted()
    {
        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.GetById(It.IsAny<Guid>()).Result)
            .Returns<Guid>(id => _mockProducts.Find(p => p.Id == id && !p.IsDeleted));

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.Delete(It.IsAny<Guid>()))
            .Callback<Guid>(id =>
            {
                var product = _mockProducts.Find(p => p.Id == id);
                if (product != null)
                {
                    product.IsDeleted = true;
                }
            })
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var productPrevState = await _mockUnitOfWork.Object.ProductRepository.GetById(productId);

        Assert.That(productPrevState, Is.Not.Null);
        if (productPrevState != null)
        {
            Assert.That(productPrevState, Is.Not.Null);
            Assert.That(productPrevState.IsDeleted, Is.False);
        }

        await _mockUnitOfWork.Object.ProductRepository.Delete(productId);
        await _mockUnitOfWork.Object.SaveChangesAsync();

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        var product = await _mockUnitOfWork.Object.ProductRepository.GetById(productId);
        Assert.That(product, Is.Null);
    }
}