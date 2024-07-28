using System.Diagnostics.CodeAnalysis;

namespace HttpContextExtensions.Attributes.Blacklist;

[ExcludeFromCodeCoverage]
public class BlacklistConfiguration(HashSet<string> ips) : IBlacklistConfiguration
{
    private HashSet<string> Ips { get; } = ips;
    private string? RedirectUrl { get; set; }

    public HashSet<string> GetIps()
    {
        return Ips;
    }

    public bool IpIsInList(string ip)
    {
        return Ips.Contains(ip);
    }

    public string? Redirect()
    {
        return RedirectUrl;
    }

    public IBlacklistConfiguration SetRedirectUrl(string url)
    {
        RedirectUrl = url;
        return this;
    }
}