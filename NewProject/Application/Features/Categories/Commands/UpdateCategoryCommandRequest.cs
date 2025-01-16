using Application.Features.Categories.Queries;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ErrorHandlingManagement;

namespace Application.Features.Categories.Commands;

public class UpdateCategoryCommandRequest : IRequest<bool>
{
    public UpdateCategoryCommandRequest(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; }

    public string Name { get; }
}

public class UpdateCategoryCommandRequestHandler : IRequestHandler<UpdateCategoryCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    public UpdateCategoryCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Id = request.Id,
            Name = request.Name
        };

        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = request.Id }, cancellationToken);

        if (result == null)
        {
            throw new ItemNotFoundException($"Category with ID {request.Id} not found");
        }

        await _unitOfWork.CategoryRepository.Update(category);

        return true;
    }
}