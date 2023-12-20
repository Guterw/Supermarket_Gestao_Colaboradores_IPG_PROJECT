using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hierarquias.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string ?Nome { get; set; }

        public string ?Cargo { get; set; }

        public virtual ICollection<Funcionarios> ?Superiores { get; set; }
        public List<Funcionarios> Subordinados { get; set; } = new List<Funcionarios>();

        public static explicit operator Funcionarios(int v)
        {
            throw new NotImplementedException();
        }
    }
}
