using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using BuyU.ViewModels;

namespace BuyU.Models
{
    public partial class BuyUContext : IdentityDbContext<ApplicationUser>
    {
        public BuyUContext(DbContextOptions<BuyUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<CartProduct> CartProduct { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Carts)
                .WithMany(t => t.Products)
                .UsingEntity<CartProduct>(
                    j => j
                        .HasOne(pt => pt.Cart)
                        .WithMany(t => t.CartProduct)
                        .HasForeignKey(pt => pt.CartId),
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(t => t.CartProduct)
                        .HasForeignKey(pt => pt.ProductId),
                    j =>
                    {
                        j.Property(pt => pt.dateTime).HasDefaultValue(DateTime.Now);
                        j.Property(pt => pt.Quantity).HasDefaultValue("1");
                        j.HasKey(t => new { t.CartId, t.ProductId });
                    }
            );

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Cart)
            .WithOne(a => a.User)
            .HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<Order>()
                .Property(s => s.Status)
                .HasDefaultValue("Under review");

                
        }






        
    }
}
