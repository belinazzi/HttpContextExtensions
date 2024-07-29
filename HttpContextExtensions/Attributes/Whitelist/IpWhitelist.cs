using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HttpContextExtensions.Attributes.Whitelist;

/// <summary>
/// Middleware to apply Whitelist on all endpoints, controllers or minimal apis.
/// </summary>
public class IpWhitelist : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var inspector = context.HttpContext.RequestServices.GetService<IInspector>();
        var whitelistConfiguration = context.HttpContext.RequestServices.GetService<IWhitelistConfiguration>();
        var remoteIp = inspector!.For(context.HttpContext).GetIp()!;
        if (whitelistConfiguration!.IpIsNotInList(remoteIp))
        {
            if (whitelistConfiguration.Redirect() is { } redirectUrl)
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