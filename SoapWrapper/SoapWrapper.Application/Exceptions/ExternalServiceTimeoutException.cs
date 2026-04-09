namespace SoapWrapper.Application.Exceptions;

public class ExternalServiceTimeoutException : Exception
{
    public ExternalServiceTimeoutException(string message) : base(message) { }
}
