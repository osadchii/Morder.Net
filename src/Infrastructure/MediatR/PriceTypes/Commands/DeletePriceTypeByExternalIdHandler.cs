using System.Net;
using AutoMapper;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.PriceTypes.Commands;

public class DeletePriceTypeByExternalIdRequest : IRequest<PriceTypeDto>
{
    public Guid ExternalId { get; set; }
}

public class DeletePriceTypeByExternalIdHandler : IRequestHandler<DeletePriceTypeByExternalIdRequest, PriceTypeDto>
{
    private readonly MContext _context;
    private readonly ILogger<DeletePriceTypeByExternalIdHandler> _logger;
    private readonly IMapper _mapper;

    public DeletePriceTypeByExternalIdHandler(MContext context, ILogger<DeletePriceTypeByExternalIdHandler> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PriceTypeDto> Handle(DeletePriceTypeByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        var priceType = await _context.PriceTypes
            .SingleOrDefaultAsync(pt => pt.ExternalId == request.ExternalId, cancellationToken);

        if (priceType is null)
        {
            throw new HttpRequestException($"Price type with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        _context.PriceTypes.Remove(priceType);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Price type with {PriceTypeExternalId} external id has been removed",
            request.ExternalId);

        return _mapper.Map<PriceTypeDto>(priceType);
    }
}