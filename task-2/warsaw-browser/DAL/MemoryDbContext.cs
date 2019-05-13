using Microsoft.EntityFrameworkCore;
using task_2.Models;

namespace task_2
{
    public class MemoryDbContext : DbContext
    {
        public MemoryDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<NotificationEntity> Notifications { get; set; }
    }
}