using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace DAL {
    public class AppDbContext : DbContext {
        public static AppDbContext Context = new AppDbContext();

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Gamemode> Gamemodes { get; set; }
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public AppDbContext() { }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().
                HasIndex(u => u.UserName).
                IsUnique();
            modelBuilder.Entity<User>().
                HasIndex(u => u.Guid).
                IsUnique();

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken)) {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving() {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries) {
                if (entry.Entity is User user) {
                    if (user.Guid.Equals(Guid.Empty)) user.Guid = Guid.NewGuid();
                }
            }
        }



    }
}