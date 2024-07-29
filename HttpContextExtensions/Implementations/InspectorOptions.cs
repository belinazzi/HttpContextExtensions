using HttpContextExtensions.Interfaces;

namespace HttpContextExtensions.Implementations;

public class InspectorOptions : IInspectorOptions
{
    /// <summary>
    /// The Provider configured.
    /// </summary>
    public IProvider Provider { get; private set; } = null!;
    
    /// <summary>
    /// The header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public string? IpHeaderName { get; private set; }
    
    /// <summary>
    /// A method to set the implementation of IProvider. Used to get information about the request IP. Optional.
    /// </summary>
    public IInspectorOptions SetIpInfoProvider(IProvider provider)
    {
        Provider = provider;
        return this;
    }

    /// <summary>
    /// A method to set the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public IInspectorOptions SetIpHeaderName(string headerName)
    {
        IpHeaderName = headerName;
        return this;
    }
    
    /// <summary>
    /// Build the Inspector object, with all configurations included.
    /// </summary>
    public IInspector BuildInspector()
    {
        return new Inspector(this);
    }
}