using HttpContextExtensions.Attributes.Whitelist;
using HttpContextExtensions.Interfaces;
using HttpContextExtensions.Middlewares.Whitelist;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HttpContextExtensionsTest;

[TestFixture]
public class WhitelistMiddlewareTests
{
    [Test]
    public async Task InvokeAsync_IpInWhitelist_ShouldRedirect()
    {
        var inspectorMock = new Mock<IInspector>();
        var whitelistConfigurationMock = new Mock<IWhitelistConfiguration>();
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        whitelistConfigurationMock.Setup(config => config.IpIsNotInList(ip)).Returns(true);
        whitelistConfigurationMock.Setup(config => config.Redirect()).Returns("http://redirect.url");
        var middleware = new IpWhitelistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, whitelistConfigurationMock.Object);
        Assert.Multiple(() =>
        {
            Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status302Found));
            Assert.That(context.Response.Headers.Location.ToString(), Is.EqualTo("http://redirect.url"));
        });
        requestDelegateMock.Verify(rd => rd.Invoke(It.IsAny<HttpContext>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_IpInWhitelist_ShouldReturn403()
    {
        var inspectorMock = new Mock<IInspector>();
        var whitelistConfigurationMock = new Mock<IWhitelistConfiguration>();
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        whitelistConfigurationMock.Setup(config => config.IpIsNotInList(ip)).Returns(true);
        whitelistConfigurationMock.Setup(config => config.Redirect()).Returns<string>(null!);
        var middleware = new IpWhitelistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, whitelistConfigurationMock.Object);
        Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
        requestDelegateMock.Verify(rd => rd.Invoke(It.IsAny<HttpContext>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_IpNotInWhitelist_ShouldCallNext()
    {
        var inspectorMock = new Mock<IInspector>();
        var whitelistConfigurationMock = new Mock<IWhitelistConfiguration>();
        if (whitelistConfigurationMock == null) throw new ArgumentNullException(nameof(whitelistConfigurationMock));
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        requestDelegateMock.Setup(rd => rd.Invoke(context)).Returns(Task.CompletedTask);
        const string ip = "127.0.0.1";
        inspectorMock.Setup(inspector => inspector.For(context).GetIp()).Returns(ip);
        whitelistConfigurationMock.Setup(config => config.IpIsNotInList(ip)).Returns(false);
        var middleware = new IpWhitelistMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context, inspectorMock.Object, whitelistConfigurationMock.Object);
        Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        requestDelegateMock.Verify(rd => rd.Invoke(context), Times.Once);
    }
}