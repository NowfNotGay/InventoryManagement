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
    public DbSet<Brand> Brands { get; set; }
    public DbSet<VehicleModel> VehicleModels { get; set; }
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

        //Hai - Brand
        modelBuilder.Entity<Brand>()
            .ToTable("Brand")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<Brand>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Brand>()
               .Property(c => c.BrandCode)
               .IsRequired()
               .HasMaxLength(100);

        modelBuilder.Entity<Brand>()
               .Property(c => c.BrandName)
               .IsRequired()
               .HasMaxLength(100);


        //Hai - VehicleModel
        modelBuilder.Entity<VehicleModel>()
            .ToTable("VehicleModel")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<VehicleModel>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<VehicleModel>()
            .Property(vm => vm.ModelCode)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<VehicleModel>()
            .Property(vm => vm.ModelName)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<VehicleModel>()
            .Property(vm => vm.BrandID)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
