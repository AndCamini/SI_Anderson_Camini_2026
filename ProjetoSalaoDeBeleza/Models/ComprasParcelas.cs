using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class ComprasParcelas
    {
        [Key]
        public int CodParcela { get; set; }

        public int CodCompra { get; set; }
        public Compras? oCompra { get; set; }

        public int Numero { get; set; }
        public DateTime Vencimento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
    }
}