using System.ComponentModel.DataAnnotations;

namespace CaseItauWeb2.Models
{
    public class Fundo
    {
        [Key]
        public string codigo { get; set; }
        public string nome { get; set; }
        public string cnpj { get; set; }
        public int codigoTipo { get; set; }
        public string nomeTipo { get; set; }
        public double patrimonio { get; set; }
    }
}
