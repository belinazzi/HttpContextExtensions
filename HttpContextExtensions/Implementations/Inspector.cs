using HttpContextExtensions.Interfaces;
using HttpContextExtensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HttpContextExtensions.Implementations;

/// <summary>
/// A class to retrieve data from HttpContext
/// </summary>
public class Inspector : IInspector
{
    private readonly IInspectorOptions _options;
    private readonly IProvider? _provider;

    /// <summary>
    /// A class to retrieve data from HttpContext
    /// </summary>
    public Inspector(IInspectorOptions options, IServiceProvider serviceProvider)
    {
        _options = options;
        _provider = serviceProvider.GetService<IProvider>();
    }

    /// <summary>
    /// The HttpContext to use.
    /// </summary>
    private HttpContext Context { get; set; } = null!;

    /// <summary>
    /// Set the HttpContext used by inspector.
    /// </summary>
    public IInspector For(HttpContext context)
    {
        Context = context;
        return this;
    }

    /// <summary>
    /// Get the request IP. If behind proxy, configure the IpHeaderName on InspectorOptions.
    /// </summary>
    public string GetIp()
    {
        var ip = string.Empty;
        if (_options.GetIpHeaderName() != null)
            ip = Context.Request.Headers[_options.GetIpHeaderName()!].FirstOrDefault();
        return string.IsNullOrEmpty(ip) ? Context.Connection.RemoteIpAddress!.ToString() : ip;
    }

    /// <summary>
    /// Get the request tracker GUID or null if not enabled.
    /// </summary>
    public Guid? GetRequestTracker()
    {
        var guid = Context.Items["RequestTracker"];
        return guid != null ? Guid.Parse(guid.ToString()!) : null;
    }

    /// <summary>
    /// Get request User Agent header.
    /// </summary>
    public string? GetUserAgent()
    {
        return Context.Request.Headers.UserAgent.FirstOrDefault();
    }

    /// <summary>
    /// Get the Request model with some information about the HttpContext.
    /// </summary>
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

    /// <summary>
    /// Get the Response model of HttpContext, effective if the Response was gerenated, generaly used inside middlewares.
    /// </summary>
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

    /// <summary>
    /// Get information about the IP, the IP Info Provider must be configured.
    /// </summary>
    public RequestIpInfo GetIpInfo()
    {
        return new RequestIpInfo
        {
            Ip = GetIp(),
            IpCountryCode = _provider?.GetCountryCode(GetIp()).Result,
            IpCountryName = _provider?.GetCountryName(GetIp()).Result,
            IpAsn = _provider?.GetAsn(GetIp()).Result,
            IpAsnName = _provider?.GetAsnName(GetIp()).Result
        };
    }

    /// <summary>
    /// Get custom data from your configured Provider.
    /// </summary>
    public T GetData<T>()
    {
        if (_provider is null)
            throw new Exception("IProvider must be configured to use this method");
        return _provider.GetData<T>(GetIp()).Result;
    }
}