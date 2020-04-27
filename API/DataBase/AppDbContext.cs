using Microsoft.EntityFrameworkCore;
using System;

using DataBase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public override int SaveChanges()
        {
            AddInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddInfo()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach(var ent in entities)
            {
                if (ent.State == EntityState.Modified)
                    ((BaseEntity)ent.Entity).ModifiedAt = DateTime.UtcNow;
                else
                    ((BaseEntity)ent.Entity).CreatedAt = DateTime.UtcNow;

            }
        }
    }
}
