namespace HttpContextExtensions.Attributes.Blacklist;

public interface IBlacklistConfiguration
{
    public HashSet<string> GetIps();
    public bool IpIsInList(string ip);
    public string? Redirect();
    public IBlacklistConfiguration SetRedirectUrl(string url);
}