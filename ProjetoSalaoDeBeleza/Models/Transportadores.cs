using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Transportadores
    {
        [Key]
        public int CodTransportador { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(14)]
        public string? CPF { get; set; }

        [MaxLength(14)]
        public string? CNPJ { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? Telefone { get; set; }

        [MaxLength(8)]
        public string? CEP { get; set; }

        [MaxLength(150)]
        public string? Rua { get; set; }

        [MaxLength(10)]
        public string? Numero { get; set; }

        [MaxLength(100)]
        public string? Complemento { get; set; }

        [MaxLength(100)]
        public string? Bairro { get; set; }

        public int CodCidade { get; set; }
        public Cidades? oCidade { get; set; }

        public bool Ativo { get; set; } = true;

        public List<Veiculos> Veiculos { get; set; } = new();
    }
}