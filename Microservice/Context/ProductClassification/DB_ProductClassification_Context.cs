using Core.ProductClassification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context.ProductClassification;
public class DB_ProductClassification_Context : DbContext
{
    public DB_ProductClassification_Context(DbContextOptions<DB_ProductClassification_Context> options) : base(options)
    {
    }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    //////////////////////
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductType>()
            .ToTable("ProductType")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<ProductType>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<ProductCategory>()
             .ToTable("ProductCategory")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<ProductCategory>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        base.OnModelCreating(modelBuilder);
    }
}
