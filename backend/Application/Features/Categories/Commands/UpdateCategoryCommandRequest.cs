using Application.Features.Categories.Queries;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Categories.Commands;

public class UpdateCategoryCommandRequest(string name) : IRequest<bool>
{
    public Guid Id { get; set; }

    public string Name { get; } = name;
}

public class UpdateCategoryCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<UpdateCategoryCommandRequest, bool>
{
    public async Task<bool> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Id = request.Id,
            Name = request.Name
        };

        await mediator.Send(new GetCategoryByIdQuery { Id = request.Id }, cancellationToken);

        await unitOfWork.CategoryRepository.Update(category);

        return true;
    }
}