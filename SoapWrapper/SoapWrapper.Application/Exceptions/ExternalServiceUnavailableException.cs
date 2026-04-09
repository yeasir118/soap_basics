namespace SoapWrapper.Application.Exceptions;

public class ExternalServiceUnavailableException : Exception
{
    public ExternalServiceUnavailableException(string message) : base(message) { }
}
