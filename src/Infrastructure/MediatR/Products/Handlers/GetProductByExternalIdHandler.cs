using AutoMapper;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class GetProductByExternalIdHandler : IRequestHandler<GetProductByExternalIdRequest, ProductDto?>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetProductByExternalIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByExternalIdRequest request, CancellationToken cancellationToken)
    {
        Product? dbEntry = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ExternalId == request.ExternalId, cancellationToken: cancellationToken);
        return _mapper.Map<ProductDto>(dbEntry);
    }
}