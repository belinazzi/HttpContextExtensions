using System.Text;
using HttpContextExtensions.Implementations;
using HttpContextExtensions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;

namespace HttpContextExtensionsTest;

[TestFixture]
public class InspectorTests
{
    [SetUp]
    public void SetUp()
    {
        _contextMock = new Mock<HttpContext>();
        _providerMock = new Mock<IProvider>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IProvider))).Returns(_providerMock.Object);
        _inspector = new Inspector(new InspectorOptions()
            .SetIpHeaderName("X-Real-IP"), _serviceProviderMock.Object);

        var dictionary = new HeaderDictionary();
        dictionary.Append(HeaderNames.Cookie, new StringValues("cookie1=value1"));
        dictionary.Append("header1", new StringValues("value1"));
        dictionary.Append("User-Agent", new StringValues(UserAgent));
        _requestFeature.Headers = dictionary;

        _featureCollection.Set<IHttpRequestFeature>(_requestFeature);

        var cookiesFeature = new RequestCookiesFeature(_featureCollection);

        _contextMock.Setup(c => c.Items["RequestTracker"]).Returns(_requestTracker.ToString());
        _contextMock.Setup(r => r.Request.Headers).Returns(dictionary);
        _contextMock.Setup(r => r.Request.Cookies).Returns(cookiesFeature.Cookies);
        _contextMock.Setup(r => r.Request.Path).Returns(Path);
        _contextMock.Setup(r => r.Request.QueryString).Returns(new QueryString(QueryString));
        _contextMock.Setup(r => r.Request.Headers.UserAgent).Returns(UserAgent);
        _contextMock.Setup(r => r.Request.Method).Returns(Method);
        _contextMock.Setup(r => r.Request.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Body)));
        _contextMock.Setup(c => c.Request.Headers).Returns(dictionary);
    }

    private Mock<HttpContext> _contextMock;
    private IInspector _inspector;
    private Mock<IProvider> _providerMock;
    private Mock<IServiceProvider> _serviceProviderMock;

    private const string Path = "/test";
    private const string QueryString = "?param1=value1";
    private const string UserAgent = "Mozilla/5.0";
    private const string Method = "POST";
    private const string Body = "{\"key\":\"value\"}";
    private readonly Guid _requestTracker = Guid.NewGuid();
    private readonly Dictionary<string, string> _cookies = new() { { "cookie1", "value1" } };

    private readonly Dictionary<string, string> _headers = new()
        { { "header1", "value1" }, { "cookie1", "value1" }, { "User-Agent", UserAgent } };

    private readonly HttpRequestFeature _requestFeature = new();
    private readonly FeatureCollection _featureCollection = new();

    [Test]
    public void GetIp_Returns_CorrectIp()
    {
        const string ipHeaderName = "X-Real-IP";
        const string ip = "192.168.1.1";
        _contextMock.Setup(r => r.Request.Headers[ipHeaderName]).Returns(new StringValues(ip));
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetIp();
        Assert.That(result, Is.EqualTo(ip));
    }

    [Test]
    public void GetRequestTracker_Returns_CorrectGuid()
    {
        var guid = Guid.NewGuid();
        _contextMock.Setup(c => c.Items["RequestTracker"]).Returns(guid.ToString());
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetRequestTracker();
        Assert.That(result, Is.EqualTo(guid));
    }

    [Test]
    public void GetUserAgent_Returns_CorrectUserAgent()
    {
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetUserAgent();
        Assert.That(result, Is.EqualTo(UserAgent));
    }

    [Test]
    public void GetRequest_Returns_CorrectRequest()
    {
        var inspector = _inspector.For(_contextMock.Object);

        var result = inspector.GetRequest();
        Assert.Multiple(() =>
        {
            Assert.That(result.RequestTracker, Is.EqualTo(_requestTracker));
            Assert.That(result.Cookies, Is.EqualTo(_cookies));
            Assert.That(result.Headers!, Has.Count.EqualTo(_headers.Count));
            Assert.That(result.Endpoint, Is.EqualTo(Path));
            Assert.That(result.QueryParameters, Is.EqualTo(QueryString));
            Assert.That(result.UserAgent, Is.EqualTo(UserAgent));
            Assert.That(result.Body, Is.EqualTo(Body));
            Assert.That(result.Method, Is.EqualTo(Method));
        });
    }


    [Test]
    public void GetResponse_Returns_CorrectResponse()
    {
        _contextMock.Setup(r => r.Response.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Body)));
        _contextMock.Setup(r => r.Response.Headers).Returns(_requestFeature.Headers);
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetResponse();
        Assert.Multiple(() =>
        {
            Assert.That(result.Headers!, Has.Count.EqualTo(_headers.Count));
            Assert.That(result.Body, Is.EqualTo(Body));
            Assert.That(result.ResponseDate, Is.GreaterThan(DateTime.MinValue));
        });
    }

    [Test]
    public void GetIpInfo_Returns_CorrectIpInfo()
    {
        const string ip = "192.168.1.1";
        const string countryCode = "US";
        const string countryName = "United States";
        const string asn = "AS12345";
        const string asnName = "ISP Inc.";
        _contextMock.Setup(r => r.Request.Headers["X-Real-IP"]).Returns(new StringValues(ip));
        _providerMock.Setup(p => p.GetCountryCode(ip)).ReturnsAsync(countryCode);
        _providerMock.Setup(p => p.GetCountryName(ip)).ReturnsAsync(countryName);
        _providerMock.Setup(p => p.GetAsn(ip)).ReturnsAsync(asn);
        _providerMock.Setup(p => p.GetAsnName(ip)).ReturnsAsync(asnName);
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetIpInfo();

        Assert.Multiple(() =>
        {
            Assert.That(result.Ip, Is.EqualTo(ip));
            Assert.That(result.IpCountryCode, Is.EqualTo(countryCode));
            Assert.That(result.IpCountryName, Is.EqualTo(countryName));
            Assert.That(result.IpAsn, Is.EqualTo(asn));
            Assert.That(result.IpAsnName, Is.EqualTo(asnName));
        });
    }

    [Test]
    public void GetData_Returns_CorrectData()
    {
        const string ip = "192.168.1.1";
        var data = new { Key = "value" };
        _contextMock.Setup(r => r.Request.Headers["X-Real-IP"]).Returns(new StringValues(ip));
        _providerMock.Setup(p => p.GetData<object>(ip)).ReturnsAsync(data);
        var inspector = _inspector.For(_contextMock.Object);
        var result = inspector.GetData<object>();
        Assert.That(result, Is.EqualTo(data));
    }
}