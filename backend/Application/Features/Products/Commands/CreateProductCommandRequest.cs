using System.ComponentModel.DataAnnotations;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Utilities;
using NewProject.Services.RabbitMqEvents;

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

public class CreateProductCommandRequestHandler : IRequestHandler<CreateProductCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntegrationEventBus _integrationEventBus;
    public CreateProductCommandRequestHandler(IUnitOfWork unitOfWork, IIntegrationEventBus integrationEventBus)
    {
        _unitOfWork = unitOfWork;
        _integrationEventBus = integrationEventBus;
    }

    public Task Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var mappingProfile = new MappingProfile<CreateProductCommandRequest, Product>();
        var product = mappingProfile.Map(request);
        product.CategoryId = request.CategoryId == Guid.Empty ? null : request.CategoryId;

        Task.Run(() =>
        {

            _unitOfWork.ProductRepository.Add(product);

            var sendMailEvent = new SendMailEvent
            {
                Recipient = "user@example.com",
                Subject = "Product Created Successfully",
                Body = $"The product '{product.Name}' was created successfully."
            };
            _integrationEventBus.PublishAsync(sendMailEvent);
        }, cancellationToken);

        return Task.CompletedTask;
    }
}