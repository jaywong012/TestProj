using System.ComponentModel.DataAnnotations;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Utilities;

namespace Application.Features.Products.Commands;

public class CreateProductCommandRequest : IRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1_000_000_000, ErrorMessage = "Price must between 1 and 1 billion")]
    public decimal Price { get; init; }

    public Guid? CategoryId { get; set; }
}

public class CreateProductCommandRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommandRequest>
{
    public async Task Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var mappingProfile = new MappingProfile<CreateProductCommandRequest, Product>();
        var product = mappingProfile.Map(request);
        product.CategoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;

        await unitOfWork.ProductRepository.Add(product);
    }
}