using HttpContextExtensions.Attributes.Whitelist;
using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Middlewares.Whitelist;

/// <summary>
/// The whitelist middleware, will apply configured rule to any endpoint.
/// </summary>
public class IpWhitelistMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// The whitelist middleware, will apply configured rule to any endpoint.
    /// </summary>
    public IpWhitelistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IInspector inspector,
        IWhitelistConfiguration whitelistConfiguration)
    {
        var remoteIp = inspector.For(context).GetIp();
        if (whitelistConfiguration.IpIsNotInList(remoteIp!))
        {
            if (whitelistConfiguration.Redirect() is { } redirectUrl)
                context.Response.Redirect(redirectUrl);
            else
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }
}