using SoapWrapper.Application.Entities;

namespace SoapWrapper.Application.Interfaces;

public interface IUserSoap
{
    Task<User?> GetUserByIdAsync(int id);
}
