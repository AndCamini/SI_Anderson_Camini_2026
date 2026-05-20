using System.ComponentModel.DataAnnotations;

namespace ProjetoSalaoDeBeleza.Models
{
    public class Paises
    {
        [Key]
        [MaxLength(5)]
        public int CodPais { get; set; }
        [MaxLength(25)]
        public string Pais { get; set; }
        [MaxLength(3)]
        public string Sigla { get; set; }
        [MaxLength(5)]
        public string DDI { get; set; }
        [MaxLength(25)]
        public string Moeda { get; set; }
    }
}
