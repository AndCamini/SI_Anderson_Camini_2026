using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class CondicaoPagamentoParcela
    {
        [Key]
        public int CodParcela { get; set; } //não existe

        public int CodCondicao { get; set; }  //chave primaria composta
        public CondicaoPagamento? oCondicao { get; set; }

        public int Numero { get; set; } // chave primaria composta

        [Range(0, 9999, ErrorMessage = "Dias não pode ser negativo.")]
        public int Dias { get; set; } = 0;

        [Range(0.01, 100, ErrorMessage = "Percentual deve ser maior que 0%.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Percentual { get; set; } = 0;
    }
}