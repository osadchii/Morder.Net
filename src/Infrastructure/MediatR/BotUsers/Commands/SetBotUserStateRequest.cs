using MediatR;

namespace Infrastructure.MediatR.BotUsers.Commands;

public class SetBotUserStateRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public string State { get; set; }
    public string StateKey { get; set; }

    public SetBotUserStateRequest(long chatId, string state, string key = null)
    {
        ChatId = chatId;
        State = state;
        StateKey = key;
    }
}