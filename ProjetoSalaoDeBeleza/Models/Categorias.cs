using System.ComponentModel.DataAnnotations;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Categorias
    {
        [Key]
        public int CodCategoria { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(50, ErrorMessage = "Nome pode ter no máximo 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public List<Produtos> Produtos { get; set; } = new();
    }
}