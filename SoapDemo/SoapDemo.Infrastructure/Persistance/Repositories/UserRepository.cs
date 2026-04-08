using Microsoft.EntityFrameworkCore;
using SoapDemo.Application.Entities;
using SoapDemo.Application.Interfaces;

namespace SoapDemo.Infrastructure.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
