using Core.ExampleClass;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Context.Example
{
    public class DB_Testing_Context : DbContext
    {
        public DB_Testing_Context(DbContextOptions<DB_Testing_Context> options) : base(options)
        {
        }
        public DbSet<MessageContent> MessageContents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageContent>()
                .ToTable("MessageContents")
                .HasKey(mc => mc.RowPointer);

            modelBuilder.Entity<MessageContent>()
                .Property(mc => mc.RowPointer)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<MessageContent>()
                      .Property(mc => mc.ID)
                      .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
