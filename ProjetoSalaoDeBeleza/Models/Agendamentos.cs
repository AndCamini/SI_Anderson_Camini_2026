using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoSalaoDeBeleza.Models
{
    public enum StatusAgendamento
    {
        Agendado,
        Confirmado,
        EmAtendimento,
        Concluido,
        Cancelado,
        NaoCompareceu
    }

    public class Agendamentos
    {
        [Key]
        public int CodAgendamento { get; set; }

        public int CodCliente { get; set; }
        public Clientes? oCliente { get; set; }

        public int CodFuncionario { get; set; }
        public Funcionarios? oFuncionario { get; set; }

        public int CodServico { get; set; }
        public Servicos? oServico { get; set; }

        public DateTime DataHoraInicio { get; set; }

        [Range(1, 720, ErrorMessage = "Duração deve ser entre 1 e 720 minutos.")]
        public int Duracao { get; set; } = 30;

        [NotMapped]
        public DateTime DataHoraFim => DataHoraInicio.AddMinutes(Duracao);

        [Range(0, 999999.99, ErrorMessage = "Valor não pode ser negativo.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [MaxLength(20)]
        public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;

        [MaxLength(500, ErrorMessage = "Observações pode ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
    }
}
