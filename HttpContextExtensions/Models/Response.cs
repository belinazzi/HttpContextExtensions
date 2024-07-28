namespace HttpContextExtensions.Models;

public class Response
{
    public Guid RequestId;
    public Dictionary<string, string>? Headers { get; set; }
    public string? Body { get; set; }
    public DateTime ResponseDate { get; set; }
}