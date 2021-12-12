using Bot.Services;
using Bot.Services.Interfaces;
using Telegram.Bot;

namespace Bot;

public static class DependencyInjection
{
    public static void AddMorderBot(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient<IMessageRouter, MessageRouter>();

        var config = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        service.AddHostedService<ConfigureWebhook>();
        service.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient
                => new TelegramBotClient(config.BotToken, httpClient));

        service.AddScoped<HandleUpdateService>();
    }
}