using AutoMapper;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Warehouses.Queries;

public class GetAllWarehousesRequest : IRequest<List<WarehouseDto>>
{
}

public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesRequest, List<WarehouseDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetAllWarehousesHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<WarehouseDto>> Handle(GetAllWarehousesRequest request, CancellationToken cancellationToken)
    {
        return _context.Warehouses
            .AsNoTracking()
            .Select(c => _mapper.Map<WarehouseDto>(c))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}