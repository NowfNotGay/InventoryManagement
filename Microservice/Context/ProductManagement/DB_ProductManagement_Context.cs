using Core.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace Context.ProductManagement;
public class DB_ProductManagement_Context : DbContext
{
    public DB_ProductManagement_Context(DbContextOptions<DB_ProductManagement_Context> options) : base(options)
    {
    }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        base.OnModelCreating(modelBuilder);
    }
}
