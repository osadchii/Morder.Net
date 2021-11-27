using AutoMapper;
using Infrastructure.Common;
using Infrastructure.MediatR.Companies.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models.Companies;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.MediatR.Companies.Handlers;

public class UpdateCompanyInformationHandler : IRequestHandler<UpdateCompanyInformation, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public UpdateCompanyInformationHandler(MContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Unit> Handle(UpdateCompanyInformation request, CancellationToken cancellationToken)
    {
        _cache.Remove(CacheKeys.CompanyInformation);

        Company? dbEntry =
            await _context.Companies.SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            return await CreateCompany(request, cancellationToken);
        }

        return await UpdateCompany(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateCompany(UpdateCompanyInformation request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Company>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Unit> UpdateCompany(Company company, UpdateCompanyInformation request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, company);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}