using SoapDemo.Application.Entities;
using SoapDemo.Application.Interfaces;

namespace SoapDemo.Application.Services;

public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }
}
