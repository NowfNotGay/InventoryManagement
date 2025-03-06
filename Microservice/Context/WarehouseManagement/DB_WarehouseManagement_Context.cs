using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.WarehouseManagement;
using Microsoft.EntityFrameworkCore;

namespace Context.WarehouseManagement
{
    public class DB_WarehouseManagement_Context : DbContext
    {
        public DB_WarehouseManagement_Context(DbContextOptions<DB_WarehouseManagement_Context> options) : base(options)
        {
        }
        public DbSet<GoodsReceiptNote> GoodsReceiptNotes { get; set; }
        public DbSet<GoodsReceiptNoteLine> GoodsReceiptNoteLines { get; set; }
        public DbSet<CurrentStock> CurrentStocks { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoodsReceiptNote>()
                .ToTable("GoodsReceiptNote")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<GoodsReceiptNote>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<GoodsReceiptNoteLine>()
                .ToTable("GoodsReceiptNoteLine")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<GoodsReceiptNoteLine>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<CurrentStock>()
                .ToTable("CurrentStock")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<CurrentStock>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            base.OnModelCreating(modelBuilder);
        }
    }
}
