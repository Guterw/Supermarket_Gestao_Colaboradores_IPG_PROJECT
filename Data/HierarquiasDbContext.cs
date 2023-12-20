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
                .HasMany(f => f.Superiores)
                .WithMany(f => f.Subordinados)
                .UsingEntity<Dictionary<string, object>>(
                    "FuncionariosSuperiores",
                    j => j
                        .HasOne<Funcionarios>()
                        .WithMany()
                        .HasForeignKey("SuperiorId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Funcionarios>()
                        .WithMany()
                        .HasForeignKey("SubordinadoId")
                        .OnDelete(DeleteBehavior.Restrict)
                );

            // Outras configurações...

            base.OnModelCreating(modelBuilder);
        }



    }

}

