namespace HttpContextExtensions.Interfaces;

public interface IProvider
{
    /// <summary>
    /// Get the country code from IP, like BR, US, CN.
    /// </summary>
    public Task<string> GetCountryCode(string ip);
    
    /// <summary>
    /// Get the country name from IP, like Brazil, United States.
    /// </summary>
    public Task<string> GetCountryName(string ip);
    
    /// <summary>
    /// Get the ASN from IP, like AS12345.
    /// </summary>
    public Task<string> GetAsn(string ip);
    
    /// <summary>
    /// Get the name of IP ASN, like HomeNet Inc.
    /// </summary>
    public Task<string> GetAsnName(string ip);
    
    /// <summary>
    /// Get a custom object from the provider, passing the return type.
    /// </summary>
    public Task<T> GetData<T>(string ip);
}