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

        base.OnModelCreating(modelBuilder);
    }
}
