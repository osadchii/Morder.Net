using AutoMapper;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;

namespace Infrastructure.Mappings;

public class BotProfile : Profile
{
    public BotProfile()
    {
        CreateMap<CreateUpdateBotUserRequest, BotUser>();
    }
}