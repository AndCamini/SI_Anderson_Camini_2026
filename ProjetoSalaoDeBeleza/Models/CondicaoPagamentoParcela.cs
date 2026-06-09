using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class CondicaoPagamentoParcela
    {
        [Key]
        public int CodParcela { get; set; }

        public int CodCondicao { get; set; }
        public CondicaoPagamento oCondicao { get; set; }

        public int Numero { get; set; }         // nº da parcela
        public int Dias { get; set; } = 0;      // dias para vencimento

        [Column(TypeName = "decimal(5,2)")]
        public decimal Percentual { get; set; } = 0; // % do valor total
    }
}