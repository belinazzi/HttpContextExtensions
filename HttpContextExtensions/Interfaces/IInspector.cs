using HttpContextExtensions.Models;
using Microsoft.AspNetCore.Http;

namespace HttpContextExtensions.Interfaces;

public interface IInspector
{
    public IInspector For(HttpContext context);
    public string? GetIp();
    public Guid? GetRequestTracker();
    public string? GetUserAgent();
    public Request GetRequest();
    public Response GetResponse();
    public RequestIpInfo GetIpInfo();
    public T GetData<T>();
}