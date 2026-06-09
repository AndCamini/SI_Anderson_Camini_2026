using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Clientes : Pessoas
    {
        public int TotalVisitas { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalGasto { get; set; } = 0;

        [MaxLength(500)]
        public string? Observacoes { get; set; }

        public DateTime? UltimaVisita { get; set; }

        public bool RecebeNotificacoes { get; set; } = true;
    }
}