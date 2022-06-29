using System.Net;
using AutoMapper;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class GetProductByArticulHandler : IRequestHandler<GetProductByArticulRequest, ProductDto?>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetProductByArticulHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByArticulRequest request, CancellationToken cancellationToken)
    {
        Product? dbEntry = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Articul == request.Articul, cancellationToken);

        if (dbEntry is null)
        {
            throw new HttpRequestException($"Product with {request.Articul} articul not found", null,
                HttpStatusCode.NotFound);
        }
        return _mapper.Map<ProductDto>(dbEntry);
    }
}