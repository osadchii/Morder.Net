using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Common;

public class UserAccessCheckRequest : IRequest<bool>
{
    public BotUser User { get; set; }
}

public class UserAccessCheckHandler : IRequestHandler<UserAccessCheckRequest, bool>
{
    private readonly string _botOwner;
    private readonly ITelegramBotClient _client;

    public UserAccessCheckHandler(IConfiguration configuration, ITelegramBotClient client)
    {
        _client = client;
        _botOwner = configuration.GetSection("BotConfiguration")["BotOwnerUserName"];
    }

    public async Task<bool> Handle(UserAccessCheckRequest request, CancellationToken cancellationToken)
    {
        if (request.User.Administrator || request.User.Verified || request.User.UserName == _botOwner)
        {
            return true;
        }

        await _client.SendTextAsync(request.User.ChatId, MessageConstants.AccessDeniedMessage);

        return false;
    }
}