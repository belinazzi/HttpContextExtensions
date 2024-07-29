<div align="center" style="text-align: center;">
<img src="./logo.png" alt="Logo" />
</div>

## Description

`HttpContextExtensions` is a C# library that provides useful features for working with `HttpContext` in ASP.NET Core. This library simplifies accessing common HTTP context information and manipulating data in web requests.

#### Compatibility

This lib can be used with .NET 6, 7 and 8

#### Contributing

Feel free to open issues requesting new features or reporting bugs.
For code contributing, fork this project and create a PR to features/your-feature.

## Features

- **Simplified Access**: Methods to access `HttpContext` data more directly and intuitively.
- **Request and response**: Simple access to common items, like headers, cookies, body, method and more.
- **Request Tracker**: Track requests and responses with automatic GUID.
- **IP Filter**: Blacklist and Whitelist IPS using a middleware or attributes.
- **IP Information**: Easy get IP information like ASN and Country with your own implementation.
- **Dependency injection**: Easy dependency injection support.

## Installation

To install the library, you can use the NuGet Package Manager:

```bash
dotnet Install-Package HttpContextExtensions
```

## Usage examples

### Base usage

```csharp
var provider = new CustomIpInfoProvider();

builder.Services.AddSingleton<IInspector>(new InspectorOptions().SetIpHeaderName("X-Real-Ip")
.SetIpInfoProvider(provider).BuildInspector());
```

**IP Header name** is **optional**, and can be configured to get real IP behind proxies like **Nginx** and **CloudFlare**.

**IP Info Provider** is **optional** too, you need to implement IProvider interface, and pass the concrete class on provider parameter.

---

### IP Info Provider

```csharp
public class CustomIpInfoProvider : IProvider
{
    public Task<string> GetCountryCode(string ip)
    {
        return Task.FromResult("BR");
    }

    public Task<string> GetCountryName(string ip)
    {
        return Task.FromResult("BRAZIL");
    }

    public Task<string> GetAsn(string ip)
    {
        return Task.FromResult("AS12345");
    }

    public Task<string> GetAsnName(string ip)
    {
        return Task.FromResult("Telefonica SA");
    }

    public Task<T> GetData<T>(string ip)
    {
        var data = MyService.getCustomInfo(ip);
        return Task.FromResult(data);
    }
}
```

The provider is a method to get information from ips, you can implement integrations
with an IP Intelligence service, like MaxMind, IpToLocation or any other.

**GetData** can be used to retrieve custom data from your provider.

---

### Blacklist and Withe List

The endpoint will return 403 Forbidden when the IP does not match the rule.
Never use Whitelist and Blacklist on the same scope.
SetRedirectUrl is optional.

##### Ips Configuration

```csharp
var blackListIps = new HashSet<string>(["x.x.x.x", "x.x.x.x"]);
builder.Services.AddSingleton<IBlacklistConfiguration>(new BlacklistConfiguration(blackListIps).SetRedirectUrl("https://google.com"));

var whiteListIps = new HashSet<string>(["x.x.x.x", "x.x.x.x"]);
builder.Services.AddSingleton<IWhitelistConfiguration>(new WhitelistConfiguration(whiteListIps).SetRedirectUrl("https://google.com"))
```

##### Endpoint based usage

```csharp
//Endpoint Based
public class MyController : ControllerBase
{

    [IpBlacklist]
    public IActionResult MyEndpoint1()
    {
        return Ok();
    }
    
    [IpWhitelist]
    public IActionResult MyEndpoint2()
    {
        return Ok();
    }
    
}
```

##### Controller based usage

```csharp
//Controller Based
[IpBlacklist]
public class MyController : ControllerBase {}

[IpWhitelist]
public class MyController : ControllerBase {}
```

##### Middleware based usage

```csharp
//Middleware Based
app.UseIpBlacklist();
app.UseIpWhitelist();
```

---

### Request tracker

The request tracker will set a GUID on the request and response.
The X-Request-Tracker header will be sent on the response.

##### Tracker configuration

```csharp
app.UseRequestTracker();
```

##### Retrieving Tracker GUID

```csharp
var guidFromInspector = inspector.For(HttpContext).GetRequestTracker();
var guidFromContext = (Guid) HttpContext.Items["RequestTracker"];
var guidFromHeader = Guid.Parse(HttpContext.Request.Headers["X-Request-Tracker"]);
        
//RESULT = 47cf1f9f-345e-4864-8b52-10a191d4f3f5
```

### Retrieving basic information

##### On a controller

```csharp
public class MyController(IInspector inspector) : ControllerBase
{
    public IActionResult MyEndpoint1()
    {
        inspector.For(HttpContext);
        var guid = inspector.GetRequestTracker();
        var ipInfo = inspector.GetIpInfo();
        var request = inspector.GetRequest();
        var response = inspector.GetResponse();
        var userAgent = inspector.GetUserAgent();
        var ip = inspector.GetIp();
        var customData = inspector.GetData<MyIpCustomIndo>();
        return Ok();
    }
}
```

##### On a middleware

```csharp
public class CustomMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var inspector = context.RequestServices.GetService<IInspector>();
        inspector.For(context);
        
        //YOUR IMPLEMENTATION
        
        context.Response.OnCompleted(() =>
        {
            inspector.For(context);
            
            //YOUR IMPLEMENTATION
            //RESPONSE IS NOW COMPLETED
            
            return Task.CompletedTask;
        });
        await next(context);
    }
}
```
