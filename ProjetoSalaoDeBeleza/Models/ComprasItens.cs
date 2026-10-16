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

        public int Quantidade { get; set; } = 1;

        [Column(TypeName = "decimal(18,6)")]
        public decimal PrecoUnitario { get; set; } = 0;

        [Column(TypeName = "decimal(18,4)")]
        public decimal DescontoPercentual { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DescontoValor { get; set; } = 0;

        [Column(TypeName = "decimal(18,6)")]
        public decimal CustoRateado { get; set; } = 0;

        [Column(TypeName = "decimal(18,6)")]
        public decimal CustoUnitario { get; set; } = 0;

        [NotMapped]
        public decimal TotalBruto => Quantidade * PrecoUnitario;

        [NotMapped]
        public decimal Total => Math.Max(Math.Round(TotalBruto - DescontoValor, 2), 0);
    }
}