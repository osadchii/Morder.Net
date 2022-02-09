using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class ToUserManagementCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int UserId { get; set; }
}

public class ToUserManagementHandler : IRequestHandler<ToUserManagementCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToUserManagementHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToUserManagementCommand request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .AsNoTracking()
            .SingleAsync(b => b.Id == request.UserId, cancellationToken);

        KeyboardBuilder menuBuilder = new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back)
            .AddLine()
            .AddButton(user.Verified ? MenuTexts.Block : MenuTexts.Verify)
            .AddLine()
            .AddButton(user.Administrator ? MenuTexts.RemoveAdministrator : MenuTexts.AddAdministrator);

        if (!user.Administrator && !user.Verified)
        {
            menuBuilder.AddLine()
                .AddButton(MenuTexts.RemoveUser);
        }

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder.Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.UserManagement,
            request.UserId.ToString()), cancellationToken);

        return Unit.Value;
    }
}