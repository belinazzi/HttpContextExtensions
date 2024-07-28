using HttpContextExtensions.Middlewares.Blacklist;
using HttpContextExtensions.Middlewares.Tracker;
using HttpContextExtensions.Middlewares.Whitelist;
using Microsoft.AspNetCore.Builder;

namespace HttpContextExtensions.Middlewares;

public static class HttpContextMiddlewareExtension
{
    public static IApplicationBuilder UseIpBlacklist(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpBlacklistMiddleware>();
    }

    public static IApplicationBuilder UseIpWhitelist(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpWhitelistMiddleware>();
    }

    public static IApplicationBuilder UseRequestTracker(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTrackerMiddleware>();
    }
}