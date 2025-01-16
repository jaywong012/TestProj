using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Categories.Commands;

public class CreateCategoryCommandRequest : IRequest<bool>
{
    public CreateCategoryCommandRequest(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public class CreateCategoryCommandRequestHandler : IRequestHandler<CreateCategoryCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Name = request.Name
        };
        await _unitOfWork.CategoryRepository.Add(category);

        return true;
    }
}