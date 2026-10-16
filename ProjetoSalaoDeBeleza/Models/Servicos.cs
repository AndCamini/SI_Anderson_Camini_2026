using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Servicos
    {
        [Key]
        public int CodServico { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres.")]
        public string Servico { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Descrição pode ter no máximo 500 caracteres.")]
        public string? Descricao { get; set; }

        public int CodTipoServico { get; set; }
        public TiposServicos? oTipoServico { get; set; }

        [Range(1, 720, ErrorMessage = "Tempo médio deve ser entre 1 e 720 minutos.")]
        public int TempoMedio { get; set; } = 30;

        [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser maior que zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Comissão deve ser entre 0% e 100%.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ComissaoPercentual { get; set; } = 0;

        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
    }
}
