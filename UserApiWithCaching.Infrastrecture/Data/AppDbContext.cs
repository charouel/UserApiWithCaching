using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserApi.Domain.Entities;

namespace UserApi.Infrastrecture.Data
{
    public class AppDbContext : DbContext
    {       
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }  // Exemple de table
    }
}
