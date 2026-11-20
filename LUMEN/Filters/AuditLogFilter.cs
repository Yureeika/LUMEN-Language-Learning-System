namespace LUMEN.Filters;

using LUMEN.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

public class AuditLogFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            var auditService = context.HttpContext.RequestServices.GetRequiredService<IAuditService>();
            var userId = context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var action = $"{context.RouteData.Values["controller"]}_{context.RouteData.Values["action"]}";
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();

                await auditService.LogActionAsync(
                    userId: userId,
                    action: action,
                    entityName: context.RouteData.Values["controller"]?.ToString(),
                    ipAddress: ipAddress
                );
            }
        }
    }
}