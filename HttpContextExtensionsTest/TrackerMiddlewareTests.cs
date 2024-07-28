using HttpContextExtensions.Middlewares.Tracker;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HttpContextExtensionsTest;

public class TrackerMiddlewareTests
{
    [Test]
    public async Task InvokeAsync_AddsRequestGuidToContextItems()
    {
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        requestDelegateMock.Setup(rd => rd(context)).Returns(Task.CompletedTask);
        var middleware = new RequestTrackerMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context);
        Assert.Multiple(() =>
        {
            Assert.That(context.Items.ContainsKey("RequestTracker"), Is.True);
            Assert.That(context.Items["RequestTracker"], Is.InstanceOf<string>());
        });
    }

    [Test]
    public async Task InvokeAsync_CallsNextMiddleware()
    {
        var context = new DefaultHttpContext();
        var requestDelegateMock = new Mock<RequestDelegate>();
        requestDelegateMock.Setup(rd => rd(context)).Returns(Task.CompletedTask);
        var middleware = new RequestTrackerMiddleware(requestDelegateMock.Object);
        await middleware.InvokeAsync(context);
        requestDelegateMock.Verify(rd => rd(context), Times.Once);
    }
}