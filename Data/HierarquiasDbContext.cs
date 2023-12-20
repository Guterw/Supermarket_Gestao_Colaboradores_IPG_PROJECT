using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hierarquias.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hierarquias.Data
{
	public class HierarquiasDbContext : DbContext
	{
        public HierarquiasDbContext(DbContextOptions<HierarquiasDbContext> options)
        : base(options)
        {
        }

        public DbSet<Cargos> Cargos { get; set; } = default!;
    public DbSet<Funcionarios> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Funcionarios>()
                .HasOne(f => f.Superior)
                .WithMany(f => f.Subordinados)
                .HasForeignKey(f => f.SuperiorId)
                .OnDelete(DeleteBehavior.Restrict); // ou .OnDelete(DeleteBehavior.Cascade), dependendo do que você precisa

            // outras configurações...

            base.OnModelCreating(modelBuilder);
        }
    }

}

