using Core.ProductProperties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context.ProductProperties;
public class DB_ProductProperties_Context : DbContext
{
    public DB_ProductProperties_Context(DbContextOptions<DB_ProductProperties_Context> options) : base(options)
    {
    }
    public DbSet<Color> Colors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>()
            .ToTable("Color")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<Color>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        base.OnModelCreating(modelBuilder);
    }
}
