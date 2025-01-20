using System.Net;
using AutoMapper;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.PriceTypes.Queries;

public class GetPriceTypeByExternalIdRequest : IRequest<PriceTypeDto>
{
    public Guid ExternalId { get; set; }
}

public class GetPriceTypeByExternalIdHandler : IRequestHandler<GetPriceTypeByExternalIdRequest, PriceTypeDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetPriceTypeByExternalIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PriceTypeDto> Handle(GetPriceTypeByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        var dbEntry = await _context.PriceTypes
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ExternalId == request.ExternalId, cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            throw new HttpRequestException($"Price type with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        return _mapper.Map<PriceTypeDto>(dbEntry);
    }
}