using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class AgendamentosService
    {
        private readonly ApplicationDbContext _context;

        public AgendamentosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public static bool OcupaHorario(StatusAgendamento status) =>
            status != StatusAgendamento.Cancelado && status != StatusAgendamento.NaoCompareceu;

        public static Agendamentos? EncontrarConflito(IEnumerable<Agendamentos> existentes, DateTime inicio, int duracao, int codAgendamentoIgnorado)
        {
            var fim = inicio.AddMinutes(duracao);
            return existentes.FirstOrDefault(a =>
                a.CodAgendamento != codAgendamentoIgnorado &&
                OcupaHorario(a.Status) &&
                a.DataHoraInicio < fim &&
                inicio < a.DataHoraFim);
        }

        public async Task<List<Agendamentos>> GetAgendamentosDoDiaAsync(DateTime dia)
        {
            var inicio = dia.Date;
            var fim = inicio.AddDays(1);

            return await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.oCliente)
                .Include(a => a.oFuncionario)
                .Include(a => a.oServico)
                .Where(a => a.DataHoraInicio >= inicio && a.DataHoraInicio < fim)
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();
        }

        public async Task AddAgendamentoAsync(Agendamentos agendamento)
        {
            Validar(agendamento);

            if (agendamento.DataHoraInicio.Date < DateTime.Today)
                throw new Exception("Não é possível agendar em uma data que já passou.");

            if (!await _context.Clientes.AnyAsync(c => c.CodPessoa == agendamento.CodCliente && c.Ativo))
                throw new Exception("Cliente não encontrado ou inativo.");

            if (!await _context.Funcionarios.AnyAsync(f => f.CodPessoa == agendamento.CodFuncionario && f.Ativo))
                throw new Exception("Funcionário não encontrado ou inativo.");

            if (!await _context.Servicos.AnyAsync(s => s.CodServico == agendamento.CodServico && s.Ativo))
                throw new Exception("Serviço não encontrado ou inativo.");

            await ValidarConflitos(agendamento);

            agendamento.oCliente = null;
            agendamento.oFuncionario = null;
            agendamento.oServico = null;
            agendamento.DataCadastro = DateTime.UtcNow;
            agendamento.DataUltimaAlteracao = DateTime.UtcNow;
            agendamento.UsuarioUltimaAlteracao = "sistema";

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAgendamentoAsync(Agendamentos agendamento)
        {
            Validar(agendamento);

            var existente = await _context.Agendamentos.FindAsync(agendamento.CodAgendamento);
            if (existente == null) throw new Exception("Agendamento não encontrado.");

            if (existente.DataHoraInicio != agendamento.DataHoraInicio && agendamento.DataHoraInicio.Date < DateTime.Today)
                throw new Exception("Não é possível remarcar para uma data que já passou.");

            await ValidarConflitos(agendamento);

            existente.CodCliente = agendamento.CodCliente;
            existente.CodFuncionario = agendamento.CodFuncionario;
            existente.CodServico = agendamento.CodServico;
            existente.DataHoraInicio = agendamento.DataHoraInicio;
            existente.Duracao = agendamento.Duracao;
            existente.Valor = agendamento.Valor;
            existente.Status = agendamento.Status;
            existente.Observacoes = agendamento.Observacoes;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAgendamentoAsync(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null) throw new Exception("Agendamento não encontrado.");

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
        }

        private static void Validar(Agendamentos agendamento)
        {
            if (agendamento.CodCliente == 0) throw new Exception("Selecione o cliente.");
            if (agendamento.CodFuncionario == 0) throw new Exception("Selecione o funcionário.");
            if (agendamento.CodServico == 0) throw new Exception("Selecione o serviço.");

            if (agendamento.DataHoraInicio == default)
                throw new Exception("Informe a data e o horário.");

            if (agendamento.Duracao < 1 || agendamento.Duracao > 720)
                throw new Exception("Duração deve ser entre 1 e 720 minutos.");

            if (agendamento.DataHoraFim.Date != agendamento.DataHoraInicio.Date)
                throw new Exception("O atendimento precisa terminar no mesmo dia em que começa.");

            if (agendamento.Valor < 0)
                throw new Exception("Valor não pode ser negativo.");

            agendamento.Observacoes = Validacoes.Limpar(agendamento.Observacoes);
            Validacoes.ValidarTamanho(agendamento.Observacoes, 500, "Observações");
        }

        private async Task ValidarConflitos(Agendamentos agendamento)
        {
            if (!OcupaHorario(agendamento.Status)) return;

            var inicioDia = agendamento.DataHoraInicio.Date;
            var fimDia = inicioDia.AddDays(1);

            var doDia = await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.oCliente)
                .Include(a => a.oFuncionario)
                .Where(a => a.DataHoraInicio >= inicioDia && a.DataHoraInicio < fimDia &&
                            (a.CodFuncionario == agendamento.CodFuncionario || a.CodCliente == agendamento.CodCliente))
                .ToListAsync();

            var conflitoFuncionario = EncontrarConflito(
                doDia.Where(a => a.CodFuncionario == agendamento.CodFuncionario),
                agendamento.DataHoraInicio, agendamento.Duracao, agendamento.CodAgendamento);

            if (conflitoFuncionario != null)
                throw new Exception($"{conflitoFuncionario.oFuncionario?.Nome} já tem um atendimento das " +
                                    $"{conflitoFuncionario.DataHoraInicio:HH:mm} às {conflitoFuncionario.DataHoraFim:HH:mm} " +
                                    $"com {conflitoFuncionario.oCliente?.Nome}.");

            var conflitoCliente = EncontrarConflito(
                doDia.Where(a => a.CodCliente == agendamento.CodCliente),
                agendamento.DataHoraInicio, agendamento.Duracao, agendamento.CodAgendamento);

            if (conflitoCliente != null)
                throw new Exception($"O cliente já tem um atendimento das {conflitoCliente.DataHoraInicio:HH:mm} " +
                                    $"às {conflitoCliente.DataHoraFim:HH:mm} com {conflitoCliente.oFuncionario?.Nome}.");
        }
    }
}
