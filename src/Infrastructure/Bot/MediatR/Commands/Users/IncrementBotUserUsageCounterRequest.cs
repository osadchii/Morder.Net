using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class IncrementBotUserUsageCounterRequest : IRequest<Unit>
{
    public int BotUserId { get; set; }
}

public class IncrementBotUserUsageCounterHandler : IRequestHandler<IncrementBotUserUsageCounterRequest, Unit>
{
    private readonly MContext _context;

    public IncrementBotUserUsageCounterHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(IncrementBotUserUsageCounterRequest request, CancellationToken cancellationToken)
    {
        var counter = await _context.BotUserUsageCounters
            .SingleOrDefaultAsync(c => c.BotUserId == request.BotUserId, cancellationToken);

        if (counter is null)
        {
            counter = new BotUserUsageCounter
            {
                Count = 1,
                LastUse = DateTime.UtcNow,
                BotUserId = request.BotUserId
            };

            await _context.BotUserUsageCounters.AddAsync(counter, cancellationToken);
        }
        else
        {
            counter.Count++;
            counter.LastUse = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}