using AutoMapper;
using Infrastructure.MediatR.PriceTypes.Queries;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Warehouses.Handlers;

public class GetPriceTypeByExternalIdHandler : IRequestHandler<GetPriceTypeByExternalIdRequest, PriceTypeDto?>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetPriceTypeByExternalIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PriceTypeDto?> Handle(GetPriceTypeByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        PriceType? dbEntry = await _context.PriceTypes
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ExternalId == request.ExternalId, cancellationToken: cancellationToken);
        return _mapper.Map<PriceTypeDto>(dbEntry);
    }
}