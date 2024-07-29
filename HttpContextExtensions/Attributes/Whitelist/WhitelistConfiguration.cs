using System.Diagnostics.CodeAnalysis;

namespace HttpContextExtensions.Attributes.Whitelist;

/// <summary>
/// Configuration for whitelist feature.
/// </summary>
[ExcludeFromCodeCoverage]
public class WhitelistConfiguration(HashSet<string> ips) : IWhitelistConfiguration
{
    /// <summary>
    /// HashSet with all ips to blacklist.
    /// </summary>
    private HashSet<string> Ips { get; } = ips;
    
    /// <summary>
    /// Redirect URL for blocked requests.
    /// </summary>
    private string? RedirectUrl { get; set; }

    /// <summary>
    /// A method to retrieve all configured IPS.
    /// </summary>
    public HashSet<string> GetIps()
    {
        return Ips;
    }

    /// <summary>
    /// Returns true if the IP is NOT on list.
    /// </summary>
    public bool IpIsNotInList(string ip)
    {
        return Ips.Contains(ip) == false;
    }

    /// <summary>
    /// Returns the redirect url.
    /// </summary>
    public string? Redirect()
    {
        return RedirectUrl;
    }

    /// <summary>
    /// Set the redirect url and returns the configuration class.
    /// </summary>
    public IWhitelistConfiguration SetRedirectUrl(string url)
    {
        RedirectUrl = url;
        return this;
    }
}