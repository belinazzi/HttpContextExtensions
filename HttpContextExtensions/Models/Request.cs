namespace HttpContextExtensions.Models;

/// <summary>
/// The request information.
/// </summary>
public class Request
{
    /// <summary>
    /// The request GUID.
    /// </summary>
    public Guid? RequestTracker { get; set; }
    
    /// <summary>
    /// The request headers.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }
    
    /// <summary>
    /// The request cookies.
    /// </summary>
    public Dictionary<string, string>? Cookies { get; set; }
    
    /// <summary>
    /// The request endpoint, like /api/endpointx.
    /// </summary>
    public string? Endpoint { get; set; }
    
    /// <summary>
    /// The request query params, like ?name=Joe&lastname=doe.
    /// </summary>
    public string? QueryParameters { get; set; }
    
    /// <summary>
    /// The request User Agent.
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// The raw request body.
    /// </summary>
    public string? Body { get; set; }
    
    /// <summary>
    /// The request method, like POST, PUT, DELETE.
    /// </summary>
    public string Method { get; set; } = null!;
}