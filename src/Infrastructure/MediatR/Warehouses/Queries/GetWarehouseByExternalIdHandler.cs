using System.Net;
using AutoMapper;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Warehouses.Queries;

public class GetWarehouseByExternalIdRequest : IRequest<WarehouseDto>
{
    public Guid ExternalId { get; set; }
}

public class GetWarehouseByExternalIdHandler : IRequestHandler<GetWarehouseByExternalIdRequest, WarehouseDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetWarehouseByExternalIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> Handle(GetWarehouseByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        Warehouse dbEntry = await _context.Warehouses
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ExternalId == request.ExternalId, cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            throw new HttpRequestException($"Warehouse with {request.ExternalId} external Id not found", null,
                HttpStatusCode.NotFound);
        }

        return _mapper.Map<WarehouseDto>(dbEntry);
    }
}