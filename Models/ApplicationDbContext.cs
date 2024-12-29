using Microsoft.EntityFrameworkCore;

namespace reactCrudBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Student> Students { get; set; }
        // Add other DbSets here
    }
}
