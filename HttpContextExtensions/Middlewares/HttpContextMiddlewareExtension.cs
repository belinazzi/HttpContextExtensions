using HttpContextExtensions.Middlewares.Blacklist;
using HttpContextExtensions.Middlewares.Tracker;
using HttpContextExtensions.Middlewares.Whitelist;
using Microsoft.AspNetCore.Builder;

namespace HttpContextExtensions.Middlewares;

/// <summary>
/// Extension method to add middlewares.
/// </summary>
public static class HttpContextMiddlewareExtension
{
    
    /// <summary>
    /// The blacklist middleware, will apply configured rule to any endpoint.
    /// </summary>
    public static IApplicationBuilder UseIpBlacklist(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpBlacklistMiddleware>();
    }

    /// <summary>
    /// The whitelist middleware, will apply configured rule to any endpoint.
    /// </summary>
    public static IApplicationBuilder UseIpWhitelist(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpWhitelistMiddleware>();
    }

    /// <summary>
    /// The request tracker middleware, will add a GUID to all requests and responses.
    /// </summary>
    public static IApplicationBuilder UseRequestTracker(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTrackerMiddleware>();
    }
}