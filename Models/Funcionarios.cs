namespace Hierarquias.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }
        public string ?Nome { get; set; }
        public string ?Sobrenome { get; set; }   
        public int CargoId { get; set; }
        public Cargos ?Cargo { get; set; }
    }
}
