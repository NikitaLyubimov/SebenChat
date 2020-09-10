using Microsoft.EntityFrameworkCore;
using System;

using Core.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrustructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<PublicKey>()
                .HasOne(pk => pk.User)
                .WithMany(u => u.PublicKeys)
                .HasForeignKey(pk => pk.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceiver)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmailConfirmToken>()
                .HasOne(ect => ect.User)
                .WithMany(u => u.EmailConfirmTokens)
                .HasForeignKey(ect => ect.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailConfirmToken> EmailConfirmTokens { get; set; }

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
