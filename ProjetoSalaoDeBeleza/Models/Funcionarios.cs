using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Funcionarios : Pessoas
    {
        [MaxLength(30)]
        public string Cargo { get; set; }           

        [Column(TypeName = "decimal(10,2)")]
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }

        public decimal ComissaoPercentual { get; set; }
    }
}
