using AutoMapper;
using Infrastructure.MediatR.PriceTypes.Commands;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.PriceTypes.Handlers;

public class UpdatePriceTypeHandler : IRequestHandler<UpdatePriceTypeRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdatePriceTypeHandler> _logger;

    public UpdatePriceTypeHandler(MContext context, IMapper mapper,
        ILogger<UpdatePriceTypeHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdatePriceTypeRequest request, CancellationToken cancellationToken)
    {
        PriceType? dbEntry =
            await _context.PriceTypes.SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            return await CreatePriceType(request, cancellationToken);
        }

        return await UpdatePriceType(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreatePriceType(UpdatePriceTypeRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<PriceType>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created price type {request.Name}");

        return Unit.Value;
    }

    private async Task<Unit> UpdatePriceType(PriceType dbEntry, UpdatePriceTypeRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Updated price type {request.Name}");

        return Unit.Value;
    }
}