using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HappyShare.Models;
using System.Threading;

namespace HappyShare.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        const string _deleteFlag = "IsDeleted";

        public DbSet<SharedItem> SharedItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<SharedItem>().ToTable("SharedItem");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Subscription>().ToTable("Subscription");
            builder.Entity<CartItem>().ToTable("CartItem");
            builder.Entity<ShoppingCart>().ToTable("ShoppingCart");

            // this property won't be explosed in model class
            builder.Entity<SharedItem>().Property<bool>(_deleteFlag);
            builder.Entity<Category>().Property<bool>(_deleteFlag);

            // Add an entity filter to filter the logic-deleted entity
            builder.Entity<SharedItem>().HasQueryFilter(product => EF.Property<bool>(product, _deleteFlag) == false);
            builder.Entity<Category>().HasQueryFilter(category => EF.Property<bool>(category, _deleteFlag) == false);

        }

        // Adjust save changes behaviour before save it
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries<SharedItem>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[_deleteFlag] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[_deleteFlag] = true;
                        break;
                }
            }
            foreach (var entry in ChangeTracker.Entries<Category>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[_deleteFlag] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[_deleteFlag] = true;
                        break;
                }
            }
        }
    }
}
