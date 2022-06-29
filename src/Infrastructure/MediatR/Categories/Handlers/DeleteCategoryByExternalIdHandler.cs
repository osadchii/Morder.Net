using System.Net;
using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Categories.Handlers;

public class DeleteCategoryByExternalIdHandler : IRequestHandler<DeleteCategoryByExternalIdRequest, CategoryDto>
{
    private readonly IIdExtractor<Category> _extractor;
    private readonly IMediator _mediator;

    public DeleteCategoryByExternalIdHandler(IIdExtractor<Category> extractor, IMediator mediator)
    {
        _extractor = extractor;
        _mediator = mediator;
    }

    public async Task<CategoryDto> Handle(DeleteCategoryByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _extractor.GetIdAsync(request.ExternalId!.Value);

        if (!id.HasValue)
        {
            throw new HttpRequestException($"Category with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        return await _mediator.Send(new DeleteCategoryByIdRequest
        {
            Id = id.Value
        }, cancellationToken);
    }
}