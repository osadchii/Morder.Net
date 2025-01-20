using System.Net;
using AutoMapper;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Categories.Handlers;

public class DeleteCategoryByIdHandler : IRequestHandler<DeleteCategoryByIdRequest, CategoryDto>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public DeleteCategoryByIdHandler(IMapper mapper, MContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<CategoryDto> Handle(DeleteCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .SingleOrDefaultAsync(c => c.Id == request.Id!.Value, cancellationToken);

        if (category is null)
        {
            throw new HttpRequestException($"Category with {request.Id} id not found", null, HttpStatusCode.NotFound);
        }

        _context.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}