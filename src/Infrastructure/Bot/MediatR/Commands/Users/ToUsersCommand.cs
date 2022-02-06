using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class ToUsersCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToUsersHandler : IRequestHandler<ToUsersCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToUsersHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToUsersCommand request, CancellationToken cancellationToken)
    {
        BotUser[] users = await _context.BotUsers
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var menuBuilder = new KeyboardBuilder();

        foreach (BotUser user in users.OrderBy(u => u.Id))
        {
            menuBuilder.AddLine();
            menuBuilder.AddButton($"{user.Id} – {user.ToString()}");
        }

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder.Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.UsersMenu), cancellationToken);

        return Unit.Value;
    }
}