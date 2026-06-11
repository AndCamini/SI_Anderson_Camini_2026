    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProjetoSalaoDeBeleza.Models;

    namespace ProjetoSalaoDeBeleza.Models
    {
        public class Pessoas
        {
            [Key]
            public int CodPessoa { get; set; }
            [MaxLength(100)]
            public string Nome { get; set; }
            [MaxLength(11)]
            public string CPF { get; set; }
            [MaxLength(50)]
            public string? Email { get; set; }
            [MaxLength(15)]
            public string? Telefone { get; set; }
            public DateTime DataNascimento { get; set; }
            public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
            public bool Ativo { get; set; } = true;
            public Cidades oCidade { get; set; }
            public int CodCidade { get; set; }
        }
    }
// adicionar endereço completo (não só a cidade)