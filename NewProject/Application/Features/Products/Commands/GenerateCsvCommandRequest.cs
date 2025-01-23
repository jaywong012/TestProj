using Application.Features.Products.Queries;
using Application.Utilities;
using MediatR;

namespace Application.Features.Products.Commands;

public class GenerateCsvCommandRequest : IRequest<string>
{
    public string? SearchKey { get; set; }
}

public class GenerateCsvCommandRequestHandler : IRequestHandler<GenerateCsvCommandRequest, string>
{
    private readonly IMediator _mediator;
    public GenerateCsvCommandRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Handle(GenerateCsvCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductListQuery query = new()
        {
            SearchKey = request.SearchKey
        };
        var products = await _mediator.Send(query, cancellationToken);

        var csv = CsvHelper.GenerateCsv(products);

        return csv;
    }
}