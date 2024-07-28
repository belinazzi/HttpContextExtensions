namespace HttpContextExtensions.Attributes.Whitelist;

public interface IWhitelistConfiguration
{
    public HashSet<string> GetIps();
    public bool IpIsNotInList(string ip);
    public string? Redirect();
    public IWhitelistConfiguration SetRedirectUrl(string url);
}