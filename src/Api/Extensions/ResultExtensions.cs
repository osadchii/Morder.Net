using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSucceeded)
        {
            return new OkObjectResult(result);
        }

        return new BadRequestObjectResult(result);
    }
}