using AutoMapper;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.PriceTypes.Queries;

public class GetAllPriceTypesRequest : IRequest<List<PriceTypeDto>>
{
}

public class GetAllPriceTypesHandler : IRequestHandler<GetAllPriceTypesRequest, List<PriceTypeDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetAllPriceTypesHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<PriceTypeDto>> Handle(GetAllPriceTypesRequest request, CancellationToken cancellationToken)
    {
        return _context.PriceTypes
            .AsNoTracking()
            .Select(c => _mapper.Map<PriceTypeDto>(c))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}