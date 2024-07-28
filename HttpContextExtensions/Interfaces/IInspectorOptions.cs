namespace HttpContextExtensions.Interfaces;

public interface IInspectorOptions
{
    public IInspectorOptions SetIpInfoProvider(IProvider provider);
    public IInspectorOptions SetIpHeaderName(string headerName);
    public IInspector BuildInspector();
}