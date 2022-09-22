using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BuyU.Models
{
    public partial class BuyUContext : DbContext
    {

        public BuyUContext(DbContextOptions<BuyUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=BuyU;Trusted_Connection=True;");
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Cart>(entity =>
        //    {
        //        entity.ToTable("Cart");

        //        entity.Property(e => e.Id).ValueGeneratedNever();

        //        entity.HasOne(d => d.User)
        //            .WithOne(c => c.Cart)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Cart_User");
        //    });

        //    modelBuilder.Entity<Category>(entity =>
        //    {
        //        entity.HasKey(e => e.CatId);

        //        entity.ToTable("Category");
        //    });

        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.ToTable("Product");

        //        entity.Property(e => e.Color).HasMaxLength(50);

        //        entity.Property(e => e.Season).HasMaxLength(50);

        //        entity.Property(e => e.Size).HasMaxLength(50);

        //        entity.HasOne(d => d.Cat)
        //            .WithMany(p => p.Products)
        //            .HasForeignKey(d => d.CatId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Product_Category");
        //    });

        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.ToTable("User");

        //        entity.Property(e => e.Country).HasMaxLength(50);

        //        entity.Property(e => e.CreatedAt)
        //            .IsRowVersion()
        //            .IsConcurrencyToken();

        //        entity.Property(e => e.Email).HasMaxLength(100);

        //        entity.Property(e => e.FirstName).HasMaxLength(50);

        //        entity.Property(e => e.LastName).HasMaxLength(50);

        //        entity.Property(e => e.Password).HasMaxLength(50);

        //        entity.Property(e => e.Phone).HasColumnType("decimal(18, 0)");
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
