namespace HttpContextExtensions.Interfaces;

public interface IProvider
{
    public Task<string> GetCountryCode(string ip);
    public Task<string> GetCountryName(string ip);
    public Task<string> GetAsn(string ip);
    public Task<string> GetAsnName(string ip);
    public Task<T> GetData<T>(string ip);
}