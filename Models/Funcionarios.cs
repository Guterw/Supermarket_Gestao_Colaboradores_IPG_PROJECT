using System.ComponentModel.DataAnnotations;

namespace Hierarquias.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }
        public string ?Nome { get; set; }
        public string ?Sobrenome { get; set; }
        [Display(Name = "Cargo")]
        public int CargoId { get; set; }
        public Cargos ?Cargo { get; set; }
    }
}
