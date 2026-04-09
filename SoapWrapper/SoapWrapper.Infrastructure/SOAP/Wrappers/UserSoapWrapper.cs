using SoapWrapper.Application.Entities;
using SoapWrapper.Application.Interfaces;

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
}
