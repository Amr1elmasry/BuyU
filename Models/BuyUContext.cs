using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BuyU.Models
{
    public partial class BuyUContext : DbContext 
    {

        public BuyUContext(DbContextOptions<BuyUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
