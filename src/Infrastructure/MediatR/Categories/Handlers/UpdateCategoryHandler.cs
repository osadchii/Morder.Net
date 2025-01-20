using AutoMapper;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Categories.Handlers;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        if (request.ParentId.HasValue && request.ParentId != Guid.Empty)
        {
            var parent =
                await _context.Categories.SingleOrDefaultAsync(c => c.ExternalId == request.ParentId.Value,
                    cancellationToken);
            if (parent is null)
            {
                throw new ArgumentException("Parent category not found");
            }

            request.Parent = parent;
        }

        var dbEntry = await _context.Categories
            .Include(c => c.Parent)
            .SingleOrDefaultAsync(c => c.ExternalId == request.ExternalId!.Value, cancellationToken: cancellationToken);


        if (dbEntry is null)
        {
            return await CreateCategory(request, cancellationToken);
        }

        return await UpdateCategory(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateCategory(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<Category>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Unit> UpdateCategory(Category dbEntry, UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}