using Core.BaseClass;
using Core.MasterData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context.MasterData;
public class DB_MasterData_Context : DbContext
{
    public DB_MasterData_Context(DbContextOptions<DB_MasterData_Context> options) : base(options)
    {
    }
    public DbSet<BusinessPartner> BusinessPartners { get; set; }
    public DbSet<StatusMaster> StatusMasters { get; set; }
    public DbSet<Warehouse> Warehouses{ get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Bao
        modelBuilder.Entity<BusinessPartner>()
            .ToTable("BusinessPartner")
            .HasKey(bp => bp.RowPointer);

        //Set default value for RowPointer
        modelBuilder.Entity<BusinessPartner>()
            .Property(bp => bp.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");


        modelBuilder.Entity<BusinessPartner>()
                .Property(bp => bp.PartnerCode)
                    .HasMaxLength(100);

        modelBuilder.Entity<BusinessPartner>()
                .Property(bp => bp.PartnerName)
                    .HasMaxLength(200);

        modelBuilder.Entity<BusinessPartner>()
                .Property(bp => bp.ContactInfo)
                    .HasMaxLength(500);

        //Hai - StatusMaster
        modelBuilder.Entity<StatusMaster>()
            .ToTable("StatusMaster")
            .HasKey(sm => sm.RowPointer);

        modelBuilder.Entity<StatusMaster>()
            .Property(sm => sm.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<StatusMaster>()
            .Property(sm => sm.StatusCode)
            .HasMaxLength(100);

        modelBuilder.Entity<StatusMaster>()
            .Property(sm => sm.StatusName)
            .HasMaxLength(200);

        modelBuilder.Entity<StatusMaster>()
            .Property(sm => sm.Description)
            .HasMaxLength(500);

        //Hai - Warehouse
        modelBuilder.Entity<Warehouse>()
            .ToTable("Warehouse")
            .HasKey(w => w.RowPointer);

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.RowPointer)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.WarehouseCode)
            .HasMaxLength(100)
            .IsRequired(); // Make WarehouseCode a required field

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.WarehouseName)
            .HasMaxLength(100);

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.Address)
            .HasMaxLength(200);

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.BinLocationCount)
            .HasDefaultValue(0)// Default value for BinLocationCount if not provided
            .HasMaxLength(500);

        modelBuilder.Entity<Warehouse>()
            .Property(w => w.AllowNegativeStock)
            .HasDefaultValue(false);

        base.OnModelCreating(modelBuilder);
    }
}
