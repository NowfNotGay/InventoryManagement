﻿using Core.ProductProperties;
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

    public DbSet<Material> Materials { get; set; }

    public DbSet<Dimension> Dimensions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>()
            .ToTable("Color")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<Color>()
            .Property(c => c.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Material>()
            .ToTable("Material")
            .HasKey(c => c.RowPointer);
        modelBuilder.Entity<Material>()
            .Property(m=>m.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Dimension>()
            .ToTable("Dimension")
            .HasKey(c => c.RowPointer);

        modelBuilder.Entity<Dimension>()
            .Property(d => d.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");


        base.OnModelCreating(modelBuilder);
    }
}
