using ProjetoSalaoDeBeleza.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Cidades
    {
        [Key]
        [MaxLength(5)]
        public int CodCidade { get; set; }
        [MaxLength(85)]
        public string Cidade { get; set; }
        public int CodEstado { get; set; }
        [ForeignKey("CodEstado")]
        public Estados oEstado { get; set; }
    }
}
