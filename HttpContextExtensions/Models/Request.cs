namespace HttpContextExtensions.Models;

public class Request
{
    public Guid? RequestTracker { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public Dictionary<string, string>? Cookies { get; set; }
    public string? Endpoint { get; set; }
    public string? QueryParameters { get; set; }
    public string? UserAgent { get; set; }
    public string? Body { get; set; }
    public string Method { get; set; } = null!;
}