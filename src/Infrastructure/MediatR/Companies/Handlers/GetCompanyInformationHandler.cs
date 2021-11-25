using AutoMapper;
using Infrastructure.Common;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.MediatR.Companies.Handlers;

public class GetCompanyInformationHandler : IRequestHandler<GetCompanyInformation, CompanyDto>
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

    public async Task<CompanyDto> Handle(GetCompanyInformation request, CancellationToken cancellationToken)
    {
        CompanyDto result;
        if (_cache.TryGetValue(CacheKeys.CompanyInformation, out result))
        {
            return result;
        }

        Company? dbEntry = await _context.Companies.SingleOrDefaultAsync(cancellationToken);
        result = _mapper.Map<CompanyDto>(dbEntry);

        _cache.Set(CacheKeys.CompanyInformation, result);

        return result;
    }
}