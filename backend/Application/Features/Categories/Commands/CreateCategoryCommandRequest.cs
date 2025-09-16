using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Categories.Commands;

public class CreateCategoryCommandRequest(string name) : IRequest<bool>
{
    public string Name { get; } = name;
}

public class CreateCategoryCommandRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommandRequest, bool>
{
    public async Task<bool> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Name = request.Name
        };
        await unitOfWork.CategoryRepository.Add(category);

        return true;
    }
}