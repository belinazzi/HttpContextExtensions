namespace HttpContextExtensions.Attributes.Blacklist;

/// <summary>
/// Configuration for blacklist feature.
/// </summary>
public interface IBlacklistConfiguration
{
    /// <summary>
    /// A method to retrieve all configured IPS.
    /// </summary>
    public HashSet<string> GetIps();
    
    /// <summary>
    /// Returns true if the IP is on list.
    /// </summary>
    public bool IpIsInList(string ip);
    
    /// <summary>
    /// Returns the redirect url.
    /// </summary>
    public string? Redirect();
    
    /// <summary>
    /// Set the redirect url and returns the configuration class.
    /// </summary>
    public IBlacklistConfiguration SetRedirectUrl(string url);
}