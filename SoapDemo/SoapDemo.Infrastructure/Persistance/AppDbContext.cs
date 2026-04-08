using Microsoft.EntityFrameworkCore;
using SoapDemo.Application.Entities;

namespace SoapDemo.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
