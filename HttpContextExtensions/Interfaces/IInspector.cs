using HttpContextExtensions.Models;
using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Interfaces;

public interface IInspector
{
    /// <summary>
    /// Set the HttpContext used by inspector.
    /// </summary>
    public IInspector For(HttpContext context);
    
    /// <summary>
    /// Get the request IP. If behind proxy, configure the IpHeaderName on InspectorOptions.
    /// </summary>
    public string? GetIp();
    
    /// <summary>
    /// Get the request tracker GUID or null if not enabled.
    /// </summary>
    public Guid? GetRequestTracker();
    
    /// <summary>
    /// Get request User Agent header.
    /// </summary>
    public string? GetUserAgent();
    
    /// <summary>
    /// Get the Request model with some information about the HttpContext.
    /// </summary>
    public Request GetRequest();
    
    /// <summary>
    /// Get the Response model of HttpContext, effective if the Response was generated, generally used inside middlewares.
    /// </summary>
    public Response GetResponse();
    
    /// <summary>
    /// Get information about the IP, the IP Info Provider must be configured.
    /// </summary>
    public RequestIpInfo GetIpInfo();
    
    /// <summary>
    /// Get custom data from your configured Provider.
    /// </summary>
    public T GetData<T>();
}