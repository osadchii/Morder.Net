using AutoMapper;
using Infrastructure.Common;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.MediatR.Companies.Handlers;

public class GetCompanyInformationHandler : BaseRequestHandler, IRequestHandler<GetCompanyInformation, CompanyDto>
{
    public GetCompanyInformationHandler(MContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper,
        cache)
    {
    }

    public async Task<CompanyDto> Handle(GetCompanyInformation request, CancellationToken cancellationToken)
    {
        if (Cache.TryGetValue(CacheKeys.CompanyInformation, out CompanyDto result))
        {
            return result;
        }

        Company? dbEntry = await Context.Companies
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        result = Mapper.Map<CompanyDto>(dbEntry);

        Cache.Set(CacheKeys.CompanyInformation, result);

        return result;
    }
}