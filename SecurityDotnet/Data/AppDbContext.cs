using Microsoft.EntityFrameworkCore;
using SecurityDotnet.Entities;

namespace SecurityDotnet.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
