using HttpContextExtensions.Attributes.Blacklist;
using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HttpContextExtensionsTest;

[TestFixture]
public class BlacklistAttributeTests
{
    [Test]
    public void OnActionExecuting_IpInBlacklist_ShouldRedirect()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(),
            new Dictionary<string, object>()!, new object());
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(httpContext).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(true);
        blacklistConfigurationMock.Setup(config => config.Redirect()).Returns("http://redirect.url");
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(inspectorMock.Object)
            .AddSingleton(blacklistConfigurationMock.Object)
            .BuildServiceProvider();
        var ipBlacklist = new IpBlacklist();
        ipBlacklist.OnActionExecuting(actionExecutingContext);
        Assert.That(actionExecutingContext.Result, Is.InstanceOf<RedirectResult>());
        var redirectResult = (RedirectResult)actionExecutingContext.Result;
        Assert.That(redirectResult.Url, Is.EqualTo("http://redirect.url"));
    }

    [Test]
    public void OnActionExecuting_IpInBlacklist_ShouldReturn403()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(),
            new Dictionary<string, object>()!, new object());
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(httpContext).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(true);
        blacklistConfigurationMock.Setup(config => config.Redirect()).Returns<string>(null!);

        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(inspectorMock.Object)
            .AddSingleton(blacklistConfigurationMock.Object)
            .BuildServiceProvider();

        var ipBlacklist = new IpBlacklist();
        ipBlacklist.OnActionExecuting(actionExecutingContext);
        Assert.That(actionExecutingContext.Result, Is.InstanceOf<ContentResult>());
        var contentResult = (ContentResult)actionExecutingContext.Result;
        Assert.That(contentResult.StatusCode, Is.EqualTo(403));
    }

    [Test]
    public void OnActionExecuting_IpNotInBlacklist_ShouldNotChangeResult()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(),
            new Dictionary<string, object>()!, new object());
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(httpContext).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(false);
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(inspectorMock.Object)
            .AddSingleton(blacklistConfigurationMock.Object)
            .BuildServiceProvider();

        var ipBlacklist = new IpBlacklist();
        ipBlacklist.OnActionExecuting(actionExecutingContext);
        Assert.That(actionExecutingContext.Result, Is.Null);
    }
}