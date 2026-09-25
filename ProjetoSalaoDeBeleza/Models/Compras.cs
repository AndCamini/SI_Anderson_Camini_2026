using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Compras
    {
        [Key]
        public int CodCompra { get; set; }

        [Required]
        public int Modelo { get; set; } = 55;

        [Required]
        public int Serie { get; set; } = 1;

        public int? NumeroNota { get; set; }

        [Required]
        public int CodFornecedor { get; set; }
        public Fornecedores? oFornecedor { get; set; }

        public int? CodTransportador { get; set; }
        public Transportadores? oTransportador { get; set; }

        public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
        public DateTime DataChegada { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Frete { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Seguro { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal OutrasDespesas { get; set; } = 0;

        public int? CodCondicaoPagamento { get; set; }
        public CondicaoPagamento? oCondicaoPagamento { get; set; }

        public int? CodFormaPagamento { get; set; }
        public FormasPagamento? oFormaPagamento { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }

        public List<ComprasItens> Itens { get; set; } = new();
        public List<ComprasParcelas> Parcelas { get; set; } = new();
    }
}