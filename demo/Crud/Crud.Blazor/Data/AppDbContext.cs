using Crud.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud.Blazor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
        : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}