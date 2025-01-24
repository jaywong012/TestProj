using Application.Features.Products.Queries;
using Application.Utilities;
using MediatR;

namespace Application.Features.Products.Commands;

public class GenerateCsvCommandRequest : IRequest<string>
{
    public string? SearchKey { get; set; }
}

public class GenerateCsvCommandRequestHandler(IMediator mediator) : IRequestHandler<GenerateCsvCommandRequest, string>
{
    public async Task<string> Handle(GenerateCsvCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductListQuery query = new()
        {
            SearchKey = request.SearchKey
        };
        var products = await mediator.Send(query, cancellationToken);

        var csv = CsvHelper.GenerateCsv(products);

        return csv;
    }
}