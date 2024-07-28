using HttpContextExtensions.Interfaces;

namespace HttpContextExtensions.Implementations;

public class InspectorOptions : IInspectorOptions
{
    public IProvider Provider { get; private set; } = null!;
    public string? IpHeaderName { get; private set; }

    public IInspectorOptions SetIpInfoProvider(IProvider provider)
    {
        Provider = provider;
        return this;
    }

    public IInspectorOptions SetIpHeaderName(string headerName)
    {
        IpHeaderName = headerName;
        return this;
    }

    public IInspector BuildInspector()
    {
        return new Inspector(this);
    }
}