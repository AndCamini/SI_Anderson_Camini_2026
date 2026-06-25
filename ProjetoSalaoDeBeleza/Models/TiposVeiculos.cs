using System.ComponentModel.DataAnnotations;

namespace ProjetoSalaoDeBeleza.Models
{
    public class TiposVeiculos
    {
        [Key]
        public int CodTipo { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}