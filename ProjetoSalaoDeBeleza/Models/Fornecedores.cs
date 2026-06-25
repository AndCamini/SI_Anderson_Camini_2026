using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Fornecedores
    {
        [Key]
        public int CodFornecedor { get; set; }

        [Required(ErrorMessage = "Razão Social é obrigatória.")]
        [MaxLength(150)]
        public string RazaoSocial { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? NomeFantasia { get; set; }

        [Required(ErrorMessage = "CNPJ é obrigatório.")]
        [MaxLength(14)]
        public string CNPJ { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? InscricaoEstadual { get; set; }

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
    }
}