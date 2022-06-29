using AutoMapper;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Queries;

public class GetAllProductsRequest : IRequest<List<ProductDto>>
{
    
}

public class GetAllProductsHandler : IRequestHandler<GetAllProductsRequest, List<ProductDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<ProductDto>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        return _context.Products
            .Include(c => c.Category)
            .AsNoTracking()
            .Select(c => _mapper.Map<ProductDto>(c))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}