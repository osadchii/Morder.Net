using System.Net;
using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Ozon.Queries;

public class GetOzonByIdRequest : IRequest<OzonDto>
{
    public int Id { get; set; }
}

public class GetOzonByIdHandler : IRequestHandler<GetOzonByIdRequest, OzonDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetOzonByIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<OzonDto> Handle(GetOzonByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _context.Marketplaces
            .AsNoTracking()
            .Include(m => m.Warehouse)
            .Include(m => m.PriceType)
            .Where(m => m.Id == request.Id && m.Type == MarketplaceType.Ozon)
            .Select(m => _mapper.Map<OzonDto>(m))
            .SingleOrDefaultAsync(cancellationToken);

        if (result is not null) return result;
        
        var message = $"Ozon with {request.Id} id not found";
        throw new HttpRequestException(message, null, HttpStatusCode.NotFound);
    }
}