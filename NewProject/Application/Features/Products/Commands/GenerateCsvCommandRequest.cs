using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Queries;
using Domain.Utilities;
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

        var csv = GenerateCsv(products);

        return csv;
    }

    private static string GenerateCsv(List<GetProductQueryResponse> products)
    {
        var csvBuilder = new StringBuilder();
        
        csvBuilder.AppendLine("Id,Name,CategoryId,LastSavedTime,CategoryName,Price");
        
        foreach (var product in products)
        {
            var csvRow = $"{product.Id},{product.Name},{product.CategoryId},{product.LastSavedTime},{product.CategoryName},{product.Price}";
            csvBuilder.AppendLine(csvRow);
        }

        return csvBuilder.ToString();
    }
}