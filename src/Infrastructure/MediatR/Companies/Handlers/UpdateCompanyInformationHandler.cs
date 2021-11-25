using AutoMapper;
using Infrastructure.MediatR.Companies.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models.Companies;

namespace Infrastructure.MediatR.Companies.Handlers;

public class UpdateCompanyInformationHandler : IRequestHandler<UpdateCompanyInformation, bool>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public UpdateCompanyInformationHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateCompanyInformation request, CancellationToken cancellationToken)
    {
        Company? dbEntry =
            await _context.Companies.SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (dbEntry is null)
        {
            return await CreateCompany(request, cancellationToken);
        }

        return await UpdateCompany(dbEntry, request, cancellationToken);
    }

    private async Task<bool> CreateCompany(UpdateCompanyInformation request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Company>(request);

        try
        {
            await _context.AddAsync(dbEntry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    private async Task<bool> UpdateCompany(Company company, UpdateCompanyInformation request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, company);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}