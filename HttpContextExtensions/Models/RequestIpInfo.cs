namespace HttpContextExtensions.Models;

/// <summary>
/// The request remote ip information.
/// </summary>
public class RequestIpInfo
{
    /// <summary>
    /// The request remote IP.
    /// </summary>
    public string Ip { get; set; } = null!;
    
    /// <summary>
    /// The country code.
    /// </summary>
    public string? IpCountryCode { get; set; }
    
    /// <summary>
    /// The country name.
    /// </summary>
    public string? IpCountryName { get; set; }
    
    /// <summary>
    /// The ASN number.
    /// </summary>
    public string? IpAsn { get; set; }
    
    /// <summary>
    /// The ASN name.
    /// </summary>
    public string? IpAsnName { get; set; }
}