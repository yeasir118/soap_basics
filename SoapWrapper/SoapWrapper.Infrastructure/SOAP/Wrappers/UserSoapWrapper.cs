using SoapWrapper.Application.Entities;
using SoapWrapper.Application.Exceptions;
using SoapWrapper.Application.Interfaces;
using System.ServiceModel;

namespace SoapWrapper.Infrastructure.SOAP.Wrappers;

public class UserSoapWrapper : IUserSoap
{
    private readonly UserSoapServiceClient _client;

    public UserSoapWrapper(UserSoapServiceClient client)
    {
        _client = client;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _client.GetUserAsync(new soapdemo.com.userservice.v1.GetUserRequest
            {
                Id = id
            });

            return new User
            {
                Id = response.Id,
                Name = response.Name
            };
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
