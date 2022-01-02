using AutoMapper;
using Infrastructure.MediatR.Categories.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Categories.Handlers;

public class
    GetChildCategoriesByParentIdHandler : IRequestHandler<GetChildrenCategoriesByParentIdRequest,
        IEnumerable<CategoryDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetChildCategoriesByParentIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetChildrenCategoriesByParentIdRequest request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories;
        if (request.ParentId.HasValue)
        {
            categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentId.HasValue && c.ParentId.Value == request.ParentId!.Value)
                .ToListAsync(cancellationToken);
        }
        else
        {
            categories = await _context.Categories
                .AsNoTracking()
                .Where(c => !c.ParentId.HasValue)
                .ToListAsync(cancellationToken);
        }

        return categories.Select(c => _mapper.Map<CategoryDto>(c));
    }
}