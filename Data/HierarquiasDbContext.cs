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

        public DbSet<Funcionarios> Funcionarios { get; set; } = default!;

    }
}

