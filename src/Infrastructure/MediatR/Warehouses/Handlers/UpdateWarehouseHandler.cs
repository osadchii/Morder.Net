using AutoMapper;
using Infrastructure.MediatR.Warehouses.Commands;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Warehouses.Handlers;

public class UpdateWarehouseHandler : IRequestHandler<UpdateWarehouseRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWarehouseHandler> _logger;

    public UpdateWarehouseHandler(MContext context, IMapper mapper,
        ILogger<UpdateWarehouseHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        Warehouse? dbEntry =
            await _context.Warehouses
                .SingleOrDefaultAsync(w => w.ExternalId == request.ExternalId,
                    cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            return await CreateWarehouse(request, cancellationToken);
        }

        return await UpdateWarehouse(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateWarehouse(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Warehouse>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created warehouse {request.Name}");

        return Unit.Value;
    }

    private async Task<Unit> UpdateWarehouse(Warehouse dbEntry, UpdateWarehouseRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Updated warehouse {request.Name}");

        return Unit.Value;
    }
}