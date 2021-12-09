using Infrastructure.Models.BotUsers;
using MediatR;

namespace Infrastructure.MediatR.BotUsers.Commands;

public class CreateUpdateBotUserRequest : IRequest<BotUser>
{
    public long ChatId { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}