using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class CondicaoPagamento
    {
        [Key]
        public int CodCondicao { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [MaxLength(50, ErrorMessage = "Descrição pode ter no máximo 50 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(1, 360, ErrorMessage = "Número de parcelas deve ser entre 1 e 360.")]
        public int NumeroParcelas { get; set; } = 1;

        [Range(0, 999, ErrorMessage = "Prazo da 1ª parcela não pode ser negativo.")]
        public int PrimeiraParcela { get; set; } = 0;

        [Range(0, 999, ErrorMessage = "Prazo entre parcelas não pode ser negativo.")]
        public int EntreParcelas { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Juros deve ser entre 0% e 100%.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Juros { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Multa deve ser entre 0% e 100%.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Multa { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Desconto deve ser entre 0% e 100%.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Desconto { get; set; } = 0;

        public bool Ativo { get; set; } = true;

        public List<CondicaoPagamentoParcela> Parcelas { get; set; } = new();
    }
}