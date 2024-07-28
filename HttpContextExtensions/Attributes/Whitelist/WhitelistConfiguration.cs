using System.Diagnostics.CodeAnalysis;

namespace HttpContextExtensions.Attributes.Whitelist;

[ExcludeFromCodeCoverage]
public class WhitelistConfiguration(HashSet<string> ips) : IWhitelistConfiguration
{
    private HashSet<string> Ips { get; } = ips;
    private string? RedirectUrl { get; set; }

    public HashSet<string> GetIps()
    {
        return Ips;
    }

    public bool IpIsNotInList(string ip)
    {
        return Ips.Contains(ip) == false;
    }

    public string? Redirect()
    {
        return RedirectUrl;
    }

    public IWhitelistConfiguration SetRedirectUrl(string url)
    {
        RedirectUrl = url;
        return this;
    }
}