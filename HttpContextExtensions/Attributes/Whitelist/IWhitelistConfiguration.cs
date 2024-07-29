namespace HttpContextExtensions.Attributes.Whitelist;

public interface IWhitelistConfiguration
{
    /// <summary>
    /// A method to retrieve all configured IPS.
    /// </summary>
    public HashSet<string> GetIps();
    
    /// <summary>
    /// Returns true if the IP is NOT on list.
    /// </summary>
    public bool IpIsNotInList(string ip);
    
    /// <summary>
    /// Returns the redirect url.
    /// </summary>
    public string? Redirect();
    
    /// <summary>
    /// Set the redirect url and returns the configuration class.
    /// </summary>
    public IWhitelistConfiguration SetRedirectUrl(string url);
}