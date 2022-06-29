using System.Net;
using Infrastructure.Cache.Interfaces;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Products.Commands;

public class DeleteProductByExternalIdRequest : IRequest<ProductDto>
{
    public Guid ExternalId { get; set; }
}

public class DeleteProductByExternalIdHandler : IRequestHandler<DeleteProductByExternalIdRequest, ProductDto>
{
    private readonly IIdExtractor<Product> _extractor;
    private readonly IMediator _mediator;

    public DeleteProductByExternalIdHandler(IIdExtractor<Product> extractor, IMediator mediator)
    {
        _extractor = extractor;
        _mediator = mediator;
    }

    public async Task<ProductDto> Handle(DeleteProductByExternalIdRequest request, CancellationToken cancellationToken)
    {
        var id = await _extractor.GetIdAsync(request.ExternalId);

        if (!id.HasValue)
        {
            throw new HttpRequestException($"Product with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        return await _mediator.Send(new DeleteProductByIdRequest()
        {
            Id = id.Value
        }, cancellationToken);
    }
}