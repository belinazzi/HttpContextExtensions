using HttpContextExtensions.Interfaces;
using HttpContextExtensions.Models;
using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Implementations;

public class Inspector(InspectorOptions options) : IInspector
{
    private HttpContext Context { get; set; } = null!;

    public IInspector For(HttpContext context)
    {
        Context = context;
        return this;
    }

    public string GetIp()
    {
        var ip = string.Empty;
        if (options.IpHeaderName != null)
            ip = Context.Request.Headers["X-Real-IP"].FirstOrDefault();
        return string.IsNullOrEmpty(ip) ? Context.Connection.RemoteIpAddress!.ToString() : ip;
    }

    public Guid? GetRequestTracker()
    {
        var guid = Context.Items["RequestTracker"];
        return guid != null ? Guid.Parse(guid.ToString()!) : null;
    }

    public string? GetUserAgent()
    {
        return Context.Request.Headers.UserAgent.FirstOrDefault();
    }

    public Request GetRequest()
    {
        string body;
        using (var reader = new StreamReader(Context.Request.Body))
        {
            body = reader.ReadToEndAsync().Result;
        }

        return new Request
        {
            RequestTracker = GetRequestTracker(),
            Cookies = Context.Request.Cookies.ToDictionary(h => h.Key, h => h.Value.ToString()),
            Headers = Context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            Endpoint = Context.Request.Path,
            QueryParameters = Context.Request.QueryString.ToString(),
            UserAgent = Context.Request.Headers.UserAgent.ToString(),
            Body = body,
            Method = Context.Request.Method.ToUpper()
        };
    }

    public Response GetResponse()
    {
        string body;
        using (var reader = new StreamReader(Context.Request.Body))
        {
            body = reader.ReadToEndAsync().Result;
        }

        return new Response
        {
            Headers = Context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            Body = body,
            ResponseDate = DateTime.UtcNow
        };
    }

    public RequestIpInfo GetIpInfo()
    {
        return new RequestIpInfo
        {
            Ip = GetIp(),
            IpCountryCode = options.Provider.GetCountryCode(GetIp()).Result,
            IpCountryName = options.Provider.GetCountryName(GetIp()).Result,
            IpAsn = options.Provider.GetAsn(GetIp()).Result,
            IpAsnName = options.Provider.GetAsnName(GetIp()).Result
        };
    }

    public T GetData<T>()
    {
        return options.Provider.GetData<T>(GetIp()).Result;
    }
}