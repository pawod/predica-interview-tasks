using task_2.Models;
using Microsoft.EntityFrameworkCore;

namespace task_2
{
    public class MemoryDbContext:DbContext
    {
        public MemoryDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<NotificationEntity> Notifications { get; set; }
    }
}