using System.ServiceModel;

namespace SoapWrapper.Infrastructure.SOAP.Config;

public static class SoapClientFactory
{
    public static BasicHttpBinding CreateBinding(SoapOptions options)
    {
        var binding = new BasicHttpBinding
        {
            MaxReceivedMessageSize = options.MaxMessageSize,
            MaxBufferSize = options.MaxMessageSize,
            SendTimeout = TimeSpan.FromSeconds(options.TimeoutSeconds),
            ReceiveTimeout = TimeSpan.FromSeconds(options.TimeoutSeconds),
            OpenTimeout = TimeSpan.FromSeconds(options.TimeoutSeconds),
            CloseTimeout = TimeSpan.FromSeconds(options.TimeoutSeconds)
        };

        binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

        return binding;
    }

    public static EndpointAddress CreateEndpoint(SoapOptions options)
    {
        return new EndpointAddress(options.Endpoint);
    }
}
