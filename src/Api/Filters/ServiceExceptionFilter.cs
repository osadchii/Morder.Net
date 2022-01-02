using System.Net;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class ServiceExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<ServiceExceptionFilter> _logger;

    public ServiceExceptionFilter(ILogger<ServiceExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is null) return;

        context.HttpContext.Response.StatusCode = 400;
        context.Result = new JsonResult(context.Exception.AsResult());
        context.ExceptionHandled = true;

        if (context.Exception is HttpRequestException exception
            && exception?.StatusCode == HttpStatusCode.NotFound)
        {
            context.HttpContext.Response.StatusCode = 404;
        }

        _logger.LogError(context.Exception, "Api Error");
    }

    public int Order => int.MaxValue - 10;
}