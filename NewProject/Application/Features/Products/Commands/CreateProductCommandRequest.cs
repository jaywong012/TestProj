using System.ComponentModel.DataAnnotations;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Products.Commands;

public class CreateProductCommandRequest : IRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1_000_000_000, ErrorMessage = "Price must between 1 and 1 billion")]
    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }
}

public class CreateProductCommandRequestHandler : IRequestHandler<CreateProductCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateProductCommandRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var categoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;
            
            
        Product product = new()
        {
            Name = request.Name,
            Price = request.Price, 
            CategoryId = categoryId
        };
        await _unitOfWork.ProductRepository.Add(product);
    }
}