using SoapDemo.Application.Entities;

namespace SoapDemo.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
}
