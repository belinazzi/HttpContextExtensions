namespace HttpContextExtensions.Interfaces;

public interface IInspectorOptions
{
    /// <summary>
    /// A method to set the implementation of IProvider. Used to get information about the request IP. Optional.
    /// </summary>
    public IInspectorOptions SetIpInfoProvider(IProvider provider);
    
    /// <summary>
    /// A method to set the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public IInspectorOptions SetIpHeaderName(string headerName);
    
    /// <summary>
    /// Build the Inspector object, with all configurations included.
    /// </summary>
    public IInspector BuildInspector();
}