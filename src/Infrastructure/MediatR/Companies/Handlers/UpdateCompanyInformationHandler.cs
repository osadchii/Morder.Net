using AutoMapper;
using Infrastructure.Cache;
using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Companies.Handlers;

public class UpdateCompanyInformationHandler : IRequestHandler<UpdateCompanyInformationRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly IIdExtractor<PriceType> _priceTypeIdExtractor;
    private readonly ILogger<UpdateCompanyInformationHandler> _logger;

    public UpdateCompanyInformationHandler(MContext context, IMapper mapper, IMemoryCache cache,
        ILogger<UpdateCompanyInformationHandler> logger, IIdExtractor<PriceType> priceTypeIdExtractor)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
        _priceTypeIdExtractor = priceTypeIdExtractor;
    }

    public async Task<Unit> Handle(UpdateCompanyInformationRequest request, CancellationToken cancellationToken)
    {
        if (request.PriceTypeExternalId.HasValue)
        {
            int? priceTypeId = await _priceTypeIdExtractor.GetIdAsync(request.PriceTypeExternalId.Value);

            if (!priceTypeId.HasValue)
            {
                throw new ArgumentException($@"Wrong price type external id: {request.PriceTypeExternalId.Value}");
            }

            request.PriceTypeId = priceTypeId.Value;
        }

        _cache.Remove(CacheKeys.CompanyInformation);

        Company? dbEntry =
            await _context.Companies.SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            return await CreateCompany(request, cancellationToken);
        }

        return await UpdateCompany(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateCompany(UpdateCompanyInformationRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Company>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created company {request.Name}");

        return Unit.Value;
    }

    private async Task<Unit> UpdateCompany(Company dbEntry, UpdateCompanyInformationRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Updated company {request.Name}");

        return Unit.Value;
    }
}