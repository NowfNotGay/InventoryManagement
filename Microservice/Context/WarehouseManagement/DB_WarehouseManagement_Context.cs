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
        public DbSet<GoodsIssueNote> GoodsIssueNotes { get; set; }
        public DbSet<GoodsIssueNoteLine> GoodsIssueNoteLines { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferDetail> StockTransferDetails { get; set; }

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

            modelBuilder.Entity<GoodsIssueNote>()
                .ToTable("GoodsIssueNote")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<GoodsIssueNote>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            modelBuilder.Entity<GoodsIssueNoteLine>()
                .ToTable("GoodsIssueNoteLine")
                .HasKey(c => c.RowPointer);

            modelBuilder.Entity<InventoryTransaction>()
                .ToTable("InventoryTransaction")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<InventoryTransaction>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockTransfer>()
                .ToTable("GoodsReceiptNote")
                .HasKey(c => c.RowPointer);
            modelBuilder.Entity<StockTransferDetail>()
                .Property(c => c.RowPointer)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            //modelBuilder.Entity<StockTransfer>()
            //    .ToTable("GoodsReceiptNoteLine")
            //    .HasKey(c => c.RowPointer);
            //modelBuilder.Entity<StockTransferDetail>()
            //    .Property(c => c.RowPointer)
            //    .HasDefaultValueSql("NEWSEQUENTIALID()");
            base.OnModelCreating(modelBuilder);
        }
    }
}
