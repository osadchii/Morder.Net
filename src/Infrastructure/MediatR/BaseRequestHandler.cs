using AutoMapper;
using Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.MediatR;

public class BaseRequestHandler
{
    protected readonly MContext Context;
    protected readonly IMemoryCache Cache;
    protected readonly IMapper Mapper;

    protected BaseRequestHandler(MContext context, IMapper mapper, IMemoryCache cache)
    {
        Context = context;
        Cache = cache;
        Mapper = mapper;
    }
}