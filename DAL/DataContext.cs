using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enum;
using Domain.Extensions;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<StatusEntity> Statuses { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusEntity>().HasData(
                    new StatusEntity { Id = 1, Name = StatusName.Added.GetDisplayName() },
                    new StatusEntity { Id = 2, Name = StatusName.InProgress.GetDisplayName() },
                    new StatusEntity { Id = 3, Name = StatusName.Complete.GetDisplayName() }
            );

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.Status)
                .WithMany(statuses => statuses.Tasks)
                .HasForeignKey(t => t.StatusId);
        }
    }
}
