using Infrastructure.Bot;
using Infrastructure.Bot.Services;
using Telegram.Bot;

namespace Api.Bot;

public static class DependencyInjection
{
    public static void AddMorderBot(this IServiceCollection service, IConfiguration configuration)
    {
        var config = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        service.AddTransient<IMessageRouter, MessageRouter>();

        service.AddHostedService<ConfigureWebhook>();
        service.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient
                => new TelegramBotClient(config.BotToken!, httpClient));

        service.AddScoped<HandleUpdateService>();
    }
}