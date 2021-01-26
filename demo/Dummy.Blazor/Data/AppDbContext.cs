using Dummy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dummy.Blazor.Data
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