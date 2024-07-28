using HttpContextExtensions.Attributes.Blacklist;
using HttpContextExtensions.Interfaces;
using HttpContextExtensions.Middlewares.Blacklist;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HttpContextExtensionsTest;

[TestFixture]
public class BlacklistMiddlewareTests
{
    [Test]
    public async Task InvokeAsync_IpInBlacklist_ShouldRedirect()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(true);
        blacklistConfigurationMock.Setup(config => config.Redirect()).Returns("http://redirect.url");
        var middleware = new IpBlacklistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, blacklistConfigurationMock.Object);
        Assert.Multiple(() =>
        {
            Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status302Found));
            Assert.That(context.Response.Headers.Location.ToString(), Is.EqualTo("http://redirect.url"));
        });
        requestDelegateMock.Verify(rd => rd.Invoke(It.IsAny<HttpContext>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_IpInBlacklist_ShouldReturn403()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(true);
        blacklistConfigurationMock.Setup(config => config.Redirect()).Returns<string>(null!);
        var middleware = new IpBlacklistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, blacklistConfigurationMock.Object);
        Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
        requestDelegateMock.Verify(rd => rd.Invoke(It.IsAny<HttpContext>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_IpNotInBlacklist_ShouldCallNext()
    {
        var inspectorMock = new Mock<IInspector>();
        var blacklistConfigurationMock = new Mock<IBlacklistConfiguration>();
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        requestDelegateMock.Setup(rd => rd.Invoke(context)).Returns(Task.CompletedTask);
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        blacklistConfigurationMock.Setup(config => config.IpIsInList(ip)).Returns(false);
        var middleware = new IpBlacklistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, blacklistConfigurationMock.Object);
        Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        requestDelegateMock.Verify(rd => rd.Invoke(context), Times.Once);
    }
}