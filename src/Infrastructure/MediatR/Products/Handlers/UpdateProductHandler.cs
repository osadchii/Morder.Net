using AutoMapper;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public UpdateProductHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
        {
            Category? parent =
                await _context.Categories.SingleOrDefaultAsync(c => c.ExternalId == request.CategoryId.Value,
                    cancellationToken);
            if (parent is null)
            {
                throw new ArgumentException("Category not found");
            }

            request.Category = parent;
        }

        Product? dbEntry = await _context.Products
            .Include(c => c.Category)
            .SingleOrDefaultAsync(c => c.ExternalId == request.ExternalId, cancellationToken: cancellationToken);


        if (dbEntry is null)
        {
            return await CreateProduct(request, cancellationToken);
        }

        return await UpdateProduct(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Product>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Unit> UpdateProduct(Product dbEntry, UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}