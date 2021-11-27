using AutoMapper;
using Infrastructure.MediatR.Categories.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Categories.Handlers;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, List<CategoryDto>>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<CategoryDto>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        return _context.Categories
            .Include(c => c.Parent)
            .AsNoTracking()
            .Select(c => _mapper.Map<CategoryDto>(c))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}