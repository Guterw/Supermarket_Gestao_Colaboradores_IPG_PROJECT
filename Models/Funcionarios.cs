using Hierarquias.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hierarquias.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cargo { get; set; }

        public int? SuperiorId { get; set; }

        [ForeignKey("SuperiorId")]
        public Funcionarios ?Superior { get; set; }

        public List<Funcionarios> Subordinados { get; set; } = new List<Funcionarios>();
    }
}
