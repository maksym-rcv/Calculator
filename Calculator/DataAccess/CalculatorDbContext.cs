using Calculator.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Calculator.DataAccess
{
    public class CalculatorDbContext : DbContext
    {
        public DbSet<CalculatorOperation> CalculatorOperations { get; set; }

        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalculatorOperation>()
                .HasKey(co => co.Id);

            modelBuilder.Entity<CalculatorOperation>()
                .Property(co => co.Number1)
                .IsRequired();

            modelBuilder.Entity<CalculatorOperation>()
                .Property(co => co.Number2)
                .IsRequired();

            modelBuilder.Entity<CalculatorOperation>()
                .Property(co => co.Operation)
                .HasMaxLength(50);

            modelBuilder.Entity<CalculatorOperation>()
                .Property(co => co.Result)
                .HasColumnType("decimal(18,2)");
        }
    }
}
