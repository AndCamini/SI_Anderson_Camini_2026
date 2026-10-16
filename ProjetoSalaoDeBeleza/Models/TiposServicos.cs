using System.ComponentModel.DataAnnotations;

namespace ProjetoSalaoDeBeleza.Models
{
    public class TiposServicos
    {
        [Key]
        public int CodTipoServico { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [MaxLength(50, ErrorMessage = "Descrição pode ter no máximo 50 caracteres.")]
        public string TipoServico { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
    }
}
