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
    private readonly IMediator _mediator;

    public DeleteCategoryByIdHandler(IMapper mapper, MContext context, IMediator mediator)
    {
        _mapper = mapper;
        _context = context;
        _mediator = mediator;
    }

    public async Task<CategoryDto> Handle(DeleteCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        Category? category = await _context.Categories
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