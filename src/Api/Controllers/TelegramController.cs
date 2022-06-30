using Infrastructure.Bot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Api.Controllers;

[AllowAnonymous]
public class TelegramController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
        [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }
}