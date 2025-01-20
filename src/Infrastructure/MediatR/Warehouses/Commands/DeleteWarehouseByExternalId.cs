using System.Net;
using AutoMapper;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Warehouses.Commands;

public class DeleteWarehouseByExternalIdRequest : IRequest<WarehouseDto>
{
    public Guid ExternalId { get; set; }
}

public class DeleteWarehouseByExternalIdHandler : IRequestHandler<DeleteWarehouseByExternalIdRequest, WarehouseDto>
{
    private readonly MContext _context;
    private readonly ILogger<DeleteWarehouseByExternalIdHandler> _logger;
    private readonly IMapper _mapper;

    public DeleteWarehouseByExternalIdHandler(MContext context, ILogger<DeleteWarehouseByExternalIdHandler> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> Handle(DeleteWarehouseByExternalIdRequest request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _context.Warehouses
            .SingleOrDefaultAsync(pt => pt.ExternalId == request.ExternalId, cancellationToken);

        if (warehouse is null)
        {
            throw new HttpRequestException($"Warehouse with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        _context.Warehouses.Remove(warehouse);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Warehouse with {WarehouseExternalId} external id has been removed",
            request.ExternalId);

        return _mapper.Map<WarehouseDto>(warehouse);
    }
}