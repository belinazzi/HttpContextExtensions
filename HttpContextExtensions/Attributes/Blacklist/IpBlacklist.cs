using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HttpContextExtensions.Attributes.Blacklist;

/// <summary>
/// Middleware to apply Blacklist on all endpoints, controllers or minimal apis.
/// </summary>
public class IpBlacklist : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var inspector = context.HttpContext.RequestServices.GetService<IInspector>();
        var blacklistConfiguration = context.HttpContext.RequestServices.GetService<IBlacklistConfiguration>();
        var remoteIp = inspector!.For(context.HttpContext).GetIp()!;
        if (blacklistConfiguration!.IpIsInList(remoteIp))
        {
            if (blacklistConfiguration.Redirect() is { } redirectUrl)
                context.Result = new RedirectResult(redirectUrl);
            else
                context.Result = new ContentResult
                {
                    StatusCode = 403
                };
            return;
        }

        base.OnActionExecuting(context);
    }
}