using Polly;
using Polly.CircuitBreaker;
using SoapWrapper.Application.Entities;
using SoapWrapper.Application.Exceptions;
using SoapWrapper.Application.Interfaces;
using SoapWrapper.Infrastructure.SOAP.Config;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SoapWrapper.Infrastructure.SOAP.Wrappers;

public class UserSoapWrapper : IUserSoap
{
    private readonly UserSoapServiceClient _client;
    private readonly IAsyncPolicy _retryPolicy;
    private readonly SoapAuthOptions _authOptions;

    public UserSoapWrapper(
        UserSoapServiceClient client,
        IAsyncPolicy retryPolicy,
        SoapAuthOptions authOptions)
    {
        _client = client;
        _retryPolicy = retryPolicy;
        _authOptions = authOptions;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            using (new OperationContextScope(_client.InnerChannel))
            {
                var header = MessageHeader.CreateHeader(
                    "ApiKey",
                    "http://soapdemo.com/auth",
                    _authOptions.ApiKey
                );

                OperationContext.Current.OutgoingMessageHeaders.Add(header);

                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    return await _client.GetUserAsync(new soapdemo.com.userservice.v1.GetUserRequest
                    {
                        Id = id
                    });
                });

                return new User
                {
                    Id = response.Id,
                    Name = response.Name
                };
            }
        }
        catch(BrokenCircuitException ex)
        {
            throw new ExternalServiceException($"Circuit open: {ex.Message}");
        }
        catch (FaultException ex)
        {
            throw new ExternalServiceException($"SOAP Fault: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            throw new ExternalServiceTimeoutException($"SOAP request timed out");
        }
        catch (CommunicationException ex)
        {
            throw new ExternalServiceUnavailableException($"SOAP Service unavailable");
        }
        catch (Exception ex)
        {
            throw new ExternalServiceException("Unexpected SOAP error");
        }
    }
}
