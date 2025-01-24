using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Application.Features.Products.Queries;

namespace Application.Features.Products.Commands;

public class UpdateProductCommandRequest(string name, decimal price, Guid? categoryId, Guid id)
    : IRequest<Product>
{
    public Guid Id { get; set; } = id;

    public string Name { get; } = name;

    public decimal Price { get; } = price;

    public Guid? CategoryId { get; set; } = categoryId;
}

public class UpdateProductCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<UpdateProductCommandRequest, Product>
{
    public async Task<Product> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductByIdQuery existProductQuery = new()
        {
            Id = request.Id
        };
        await mediator.Send(existProductQuery, cancellationToken);

        var categoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;

        Product product = new()
        {
            Id = request.Id,
            Price = request.Price,
            Name = request.Name,
            CategoryId = categoryId
        };
        await unitOfWork.ProductRepository.Update(product);
        return product;
    }
}