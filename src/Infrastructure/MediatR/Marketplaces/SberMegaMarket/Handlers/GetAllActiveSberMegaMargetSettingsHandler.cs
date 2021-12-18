using AutoMapper;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Queries;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.SberMegaMarket.Handlers;

public class
    GetAllActiveSberMegaMargetSettingsHandler : IRequestHandler<GetAllActiveSberMegaMarketSettingsRequest,
        List<SberMegaMarketDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetAllActiveSberMegaMargetSettingsHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<SberMegaMarketDto>> Handle(GetAllActiveSberMegaMarketSettingsRequest request,
        CancellationToken cancellationToken)
    {
        return _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive && m.Type == MarketplaceType.SberMegaMarket)
            .Select(m => _mapper.Map<SberMegaMarketDto>(m))
            .ToListAsync(cancellationToken);
    }
}