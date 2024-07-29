namespace HttpContextExtensions.Models;

/// <summary>
/// The response information.
/// </summary>
public class Response
{
    /// <summary>
    /// The response GUID.
    /// </summary>
    public Guid RequestId;
    
    /// <summary>
    /// The response headers.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }
    
    /// <summary>
    /// The response body.
    /// </summary>
    public string? Body { get; set; }
    
    /// <summary>
    /// The response date (UTC).
    /// </summary>
    public DateTime ResponseDate { get; set; }
}