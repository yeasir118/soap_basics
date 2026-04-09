using SoapWrapper.Application.Entities;
using SoapWrapper.Application.Interfaces;

namespace SoapWrapper.Application.Services;

public class UserService
{
    private readonly IUserSoap _userSoap;

    public UserService(IUserSoap userSoap)
    {
        _userSoap = userSoap;
    }

    public async Task<User?> GetUser(int id)
    {
        return await _userSoap.GetUserByIdAsync(id);
    }
}
