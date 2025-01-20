using AutoMapper;
using Infrastructure.MediatR.Categories.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Categories.Handlers;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdRequest, CategoryDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(MContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .SingleAsync(c => c.Id == request.Id, cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}