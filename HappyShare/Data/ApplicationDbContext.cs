using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HappyShare.Models;

namespace HappyShare.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
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
        }
    }
}
