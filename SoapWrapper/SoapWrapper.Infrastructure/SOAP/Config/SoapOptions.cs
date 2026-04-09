namespace SoapWrapper.Infrastructure.SOAP.Config;

public class SoapOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxMessageSize { get; set; } = 65536;
}
