using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Application.Features.Products.Queries;
using Domain.ErrorHandlingManagement;

namespace Application.Features.Products.Commands;

public class UpdateProductCommandRequest : IRequest<Product>
{
    public UpdateProductCommandRequest(string name, decimal price, Guid? categoryId, Guid id)
    {
        Id = id;
        Name = name;
        Price = price;
        CategoryId = categoryId;
    }

    public Guid Id { get; set; }

    public string Name { get; }

    public decimal Price { get; }

    public Guid? CategoryId { get; }
}

public class UpdateProductCommandRequestHandler : IRequestHandler<UpdateProductCommandRequest, Product>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public UpdateProductCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Product> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductByIdQuery existProductQuery = new()
        {
            Id = request.Id
        };
        var existProduct = await _mediator.Send(existProductQuery, cancellationToken);
        if (existProduct == null)
        {
            throw new ItemNotFoundException($"Product with ID {request.Id} not found");
        }

        var categoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;

        Product product = new()
        {
            Id = request.Id,
            Price = request.Price,
            Name = request.Name,
            CategoryId = categoryId
        };
        await _unitOfWork.ProductRepository.Update(product);
        return product;
    }
}