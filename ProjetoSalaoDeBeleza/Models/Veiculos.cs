using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Veiculos
    {
        [Key]
        public int CodVeiculo { get; set; }

        [Required(ErrorMessage = "Placa é obrigatória.")]
        [MaxLength(8)]
        public string Placa { get; set; } = string.Empty;

        public bool PlacaMercosul { get; set; } = false;

        [Required(ErrorMessage = "Modelo é obrigatório.")]
        [MaxLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Cor { get; set; }

        public int Ano { get; set; }

        public int CodMarca { get; set; }
        public MarcasVeiculos? oMarca { get; set; }

        public int CodTipo { get; set; }
        public TiposVeiculos? oTipo { get; set; }

        public int CodTransportador { get; set; }
        public Transportadores? oTransportador { get; set; }

        public bool Ativo { get; set; } = true;
    }
}