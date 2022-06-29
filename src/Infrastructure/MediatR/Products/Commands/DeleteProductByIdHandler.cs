using System.Net;
using AutoMapper;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Products.Commands;

public class DeleteProductByIdRequest : IRequest<ProductDto>
{
    public int Id { get; set; }
}

public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdRequest, ProductDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductByIdHandler> _logger;

    public DeleteProductByIdHandler(MContext context, IMapper mapper, ILogger<DeleteProductByIdHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(DeleteProductByIdRequest request, CancellationToken cancellationToken)
    {
        Product? product = await _context.Products
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new HttpRequestException($"Product with {request.Id} id not found", null, HttpStatusCode.NotFound);
        }

        _context.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Deleted product with {Id} id and {Articul} articul", product.Id, product.Articul);

        return _mapper.Map<ProductDto>(product);
    }
}