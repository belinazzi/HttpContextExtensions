using HttpContextExtensions.Interfaces;

namespace HttpContextExtensions.Implementations;

public class InspectorOptions : IInspectorOptions
{
    /// <summary>
    /// The header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public string? IpHeaderName { get; private set; }
    
  
    /// <summary>
    /// A method to set the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public IInspectorOptions SetIpHeaderName(string headerName)
    {
        IpHeaderName = headerName;
        return this;
    }

    /// <summary>
    /// A method to get the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public string? GetIpHeaderName()
    {
        return IpHeaderName;
    }
}