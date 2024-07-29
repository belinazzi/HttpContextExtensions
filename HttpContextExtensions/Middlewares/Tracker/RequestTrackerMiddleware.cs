using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Middlewares.Tracker;

/// <summary>
/// The request tracker middleware, will add a GUID to all requests and responses.
/// </summary>
public class RequestTrackerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var requestGuid = Guid.NewGuid().ToString();
        context.Items["RequestTracker"] = requestGuid;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-Request-Tracker", requestGuid);
            return Task.CompletedTask;
        });
        await next(context);
    }
}