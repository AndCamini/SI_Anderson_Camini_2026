using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class ComprasItens
    {
        [Key]
        public int CodItem { get; set; }

        public int Modelo { get; set; }
        public int Serie { get; set; }
        public int NumeroNota { get; set; }
        public int CodFornecedor { get; set; }
        public Compras? oCompra { get; set; }

        public int CodProduto { get; set; }
        public Produtos? oProduto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Desconto { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total => Math.Max(Quantidade * PrecoUnitario - Desconto, 0);
    }
}