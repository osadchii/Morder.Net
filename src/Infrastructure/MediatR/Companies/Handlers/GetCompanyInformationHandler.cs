using AutoMapper;
using Infrastructure.Cache;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.MediatR.Companies.Handlers;

public class GetCompanyInformationHandler : IRequestHandler<GetCompanyInformationRequest, CompanyDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public GetCompanyInformationHandler(MContext context, IMapper mapper, IMemoryCache cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<CompanyDto> Handle(GetCompanyInformationRequest request, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(CacheKeys.CompanyInformation, out CompanyDto result))
        {
            return result;
        }

        Company dbEntry = await _context.Companies
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        result = _mapper.Map<CompanyDto>(dbEntry);

        _cache.Set(CacheKeys.CompanyInformation, result);

        return result;
    }
}