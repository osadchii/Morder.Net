using AutoMapper;
using Infrastructure.Common;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Products.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, Result>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(MContext context, IMapper mapper, ILogger<UpdateProductHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
        {
            Category parent =
                await _context.Categories.SingleOrDefaultAsync(c => c.ExternalId == request.CategoryId.Value,
                    cancellationToken);
            if (parent is null)
            {
                throw new ArgumentException("Category not found");
            }

            request.Category = parent;
        }

        var unUniqueArticul = await _context.Products
            .AsNoTracking()
            .AnyAsync(p => p.Articul == request.Articul && p.ExternalId != request.ExternalId, cancellationToken);

        if (unUniqueArticul)
        {
            var message = $"Trying to post product with non-unique articul: {request.Articul}";
            _logger.LogWarning(message);
            return ResultCode.Error.AsResult(message);
        }

        Product dbEntry = await _context.Products
            .Include(c => c.Category)
            .SingleOrDefaultAsync(c => c.ExternalId == request.ExternalId, cancellationToken: cancellationToken);


        if (dbEntry is null)
        {
            return await CreateProduct(request, cancellationToken);
        }

        return await UpdateProduct(dbEntry, request, cancellationToken);
    }

    private async Task<Result> CreateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Product>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created product {ProductName} ({ProductArticul}) with id {ProductId}", 
            dbEntry.Name, dbEntry.Articul, dbEntry.Id);

        return dbEntry.AsResult();
    }

    private async Task<Result> UpdateProduct(Product dbEntry, UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated product {ProductName} ({ProductArticul}) with id {ProductId}", 
            dbEntry.Name, dbEntry.Articul, dbEntry.Id);

        return dbEntry.AsResult();
    }
}