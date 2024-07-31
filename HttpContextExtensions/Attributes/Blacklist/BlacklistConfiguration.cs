using System.Diagnostics.CodeAnalysis;

namespace HttpContextExtensions.Attributes.Blacklist;

/// <summary>
/// Configuration for blacklist feature.
/// </summary>
[ExcludeFromCodeCoverage]
public class BlacklistConfiguration : IBlacklistConfiguration
{
    /// <summary>
    /// Configuration for blacklist feature.
    /// </summary>
    public BlacklistConfiguration(HashSet<string> ips)
    {
        Ips = ips;
    }

    /// <summary>
    /// HashSet with all ips to blacklist.
    /// </summary>
    private HashSet<string> Ips { get; }
    
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
    /// Returns true if the IP is on list.
    /// </summary>
    public bool IpIsInList(string ip)
    {
        return Ips.Contains(ip);
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
    public IBlacklistConfiguration SetRedirectUrl(string url)
    {
        RedirectUrl = url;
        return this;
    }
}