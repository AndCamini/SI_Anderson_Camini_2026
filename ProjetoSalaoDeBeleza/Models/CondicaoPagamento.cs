using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class CondicaoPagamento
    {
        [Key]
        public int CodCondicao { get; set; }

        [Required, MaxLength(50)]
        public string Descricao { get; set; }

        public int NumeroParcelas { get; set; } = 1;

        public int PrimeiraParcela { get; set; } = 0;

        public int EntreParcelas { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Juros { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Multa { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Desconto { get; set; } = 0;

        public bool Ativo { get; set; } = true;

        public List<CondicaoPagamentoParcela> Parcelas { get; set; } = new();
    }
}