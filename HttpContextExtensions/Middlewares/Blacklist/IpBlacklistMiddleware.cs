using HttpContextExtensions.Attributes.Blacklist;
using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Middlewares.Blacklist;

/// <summary>
/// The blacklist middleware, will apply configured rule to any endpoint.
/// </summary>
public class IpBlacklistMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// The blacklist middleware, will apply configured rule to any endpoint.
    /// </summary>
    public IpBlacklistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IInspector inspector,
        IBlacklistConfiguration blacklistConfiguration)
    {
        var remoteIp = inspector.For(context).GetIp();
        if (blacklistConfiguration.IpIsInList(remoteIp!))
        {
            if (blacklistConfiguration.Redirect() is { } redirectUrl)
                context.Response.Redirect(redirectUrl);
            else
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }
}