using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Orders.Commands;

public class HandlerCallbackQueryRequest : IRequest<Unit>
{
    public int BotUserId { get; set; }
    public string Data { get; set; }
}

public class HandlerCallbackQueryRequestHandler : IRequestHandler<HandlerCallbackQueryRequest, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly MContext _context;

    public HandlerCallbackQueryRequestHandler(ITelegramBotClient telegramBotClient, MContext context)
    {
        _telegramBotClient = telegramBotClient;
        _context = context;
    }

    public async Task<Unit> Handle(HandlerCallbackQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.BotUsers
            .AsNoTracking()
            .Where(x => x.Id == request.BotUserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        await _telegramBotClient.SendTextMessageAsync(user.ChatId, request.Data, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}