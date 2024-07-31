namespace HttpContextExtensions.Interfaces;

public interface IInspectorOptions
{
    /// <summary>
    /// A method to set the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public IInspectorOptions SetIpHeaderName(string headerName);
    
    /// <summary>
    /// A method to get the header name to use when getting the IP behind a proxy. Optional.
    /// </summary>
    public string? GetIpHeaderName();
}