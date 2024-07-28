namespace HttpContextExtensions.Models;

public class RequestIpInfo
{
    public string Ip { get; set; } = null!;
    public string? IpCountryCode { get; set; }
    public string? IpCountryName { get; set; }
    public string? IpAsn { get; set; }
    public string? IpAsnName { get; set; }
}