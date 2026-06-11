using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Produtos
    {
        [Key]
        public int CodProduto { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres.")]
        public string Produto { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Descrição pode ter no máximo 500 caracteres.")]
        public string? Descricao { get; set; }

        [Range(0, 999999.99, ErrorMessage = "Preço de custo não pode ser negativo.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoCusto { get; set; } = 0;

        [Range(0.01, 999999.99, ErrorMessage = "Preço de venda deve ser maior que zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoVenda { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Estoque não pode ser negativo.")]
        public int Estoque { get; set; } = 0;

        [Required(ErrorMessage = "Unidade de medida é obrigatória.")]
        [MaxLength(10, ErrorMessage = "Unidade de medida pode ter no máximo 10 caracteres.")]
        public string UnidadeMedida { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public int CodCategoria { get; set; }
        public Categorias? oCategoria { get; set; }
    }
}