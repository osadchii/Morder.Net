using AutoMapper;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.BotUsers.Handlers;

public class CreateUpdateBotUserHandler : IRequestHandler<CreateUpdateBotUserRequest, BotUser>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUpdateBotUserHandler> _logger;

    public CreateUpdateBotUserHandler(MContext context, IMapper mapper, ILogger<CreateUpdateBotUserHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BotUser> Handle(CreateUpdateBotUserRequest request, CancellationToken cancellationToken)
    {
        BotUser? dbEntry = await _context.BotUsers
            .SingleOrDefaultAsync(b => b.ChatId == request.ChatId, cancellationToken);

        if (dbEntry is null)
        {
            return await CreateUser(request, cancellationToken);
        }

        return await UpdateUser(dbEntry, request, cancellationToken);
    }

    private async Task<BotUser> CreateUser(CreateUpdateBotUserRequest request, CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<BotUser>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created bot user {User}", dbEntry.ToString());

        return dbEntry;
    }

    private async Task<BotUser> UpdateUser(BotUser dbEntry, CreateUpdateBotUserRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated bot user {User}", dbEntry.ToString());

        return dbEntry;
    }
}