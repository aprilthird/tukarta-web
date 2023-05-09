using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TUKARTA.PE.ENTITIES.Base;
using TUKARTA.PE.ENTITIES.Models;

namespace TUKARTA.PE.DATA.Context
{
    public class TuKartaDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>,
                            ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
                            IdentityUserToken<Guid>>
    {
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishCategory> DishCategories { get; set; }

        public DbSet<MenuCategory> MenuCategories { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Review> Reviews { get; set; } 

        public DbSet<ServiceSchedule> ServiceSchedules { get; set; }

        public TuKartaDbContext(DbContextOptions<TuKartaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ServiceSchedule>(serviceSchedule =>
            {
                serviceSchedule.HasKey(ss => new { ss.RestaurantId, ss.Day });

                serviceSchedule.HasOne(ss => ss.Restaurant)
                    .WithMany(ss => ss.ServiceSchedules)
                    .HasForeignKey(ss => ss.RestaurantId)
                    .IsRequired();
            });

            var modelEntityTypes = builder.Model.GetEntityTypes();
            foreach (var modelEntityType in modelEntityTypes)
            {
                foreach (var foreignKey in modelEntityType.GetForeignKeys())
                {
                    if (!foreignKey.IsOwnership && foreignKey.DeleteBehavior == DeleteBehavior.Cascade)
                    {
                        foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is BaseEntity || e.Entity is ApplicationUser) && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if(entityEntry.Entity is BaseEntity)
                {
                    ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                    }
                }
                else
                {
                    ((ApplicationUser)entityEntry.Entity).UpdatedAt = DateTime.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        ((ApplicationUser)entityEntry.Entity).CreatedAt = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}