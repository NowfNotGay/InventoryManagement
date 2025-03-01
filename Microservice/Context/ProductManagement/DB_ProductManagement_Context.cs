﻿using Core.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace Context.ProductManagement;
public class DB_ProductManagement_Context : DbContext
{
    public DB_ProductManagement_Context(DbContextOptions<DB_ProductManagement_Context> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Product
        modelBuilder.Entity<Product>()
            .ToTable("Product")
            .HasKey(c => c.RowPointer);
        modelBuilder.Entity<Product>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        //Product Variant
        modelBuilder.Entity<ProductVariant>()
            .ToTable("ProductVariant")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<ProductVariant>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<ProductVariant>()
            .Property(c => c.ProductVariantCode)
            .HasMaxLength(100);

        modelBuilder.Entity<ProductVariant>()
            .Property(c => c.Attributes)
            .HasMaxLength(500);

        //Product Attribute
        modelBuilder.Entity<ProductAttribute>()
            .ToTable("ProductAttribute")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<ProductAttribute>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        base.OnModelCreating(modelBuilder);
    }
}
