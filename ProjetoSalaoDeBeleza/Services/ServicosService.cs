using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class ServicosService
    {
        private readonly ApplicationDbContext _context;

        public ServicosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Servicos>> GetServicosAsync() =>
            await _context.Servicos
                .AsNoTracking()
                .Include(s => s.oTipoServico)
                .OrderBy(s => s.Servico)
                .ToListAsync();

        public async Task AddServicoAsync(Servicos servico)
        {
            Normalizar(servico);
            Validar(servico);

            if (await _context.Servicos.AnyAsync(s => s.Servico == servico.Servico))
                throw new Exception("Já existe um serviço com esse nome.");

            servico.oTipoServico = null;
            servico.DataCadastro = DateTime.UtcNow;
            servico.DataUltimaAlteracao = DateTime.UtcNow;
            servico.UsuarioUltimaAlteracao = "sistema";

            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServicoAsync(Servicos servico)
        {
            Normalizar(servico);
            Validar(servico);

            if (await _context.Servicos.AnyAsync(s => s.Servico == servico.Servico && s.CodServico != servico.CodServico))
                throw new Exception("Já existe um serviço com esse nome.");

            var existente = await _context.Servicos.FindAsync(servico.CodServico);
            if (existente == null) throw new Exception("Serviço não encontrado.");

            existente.Servico = servico.Servico;
            existente.Descricao = servico.Descricao;
            existente.CodTipoServico = servico.CodTipoServico;
            existente.TempoMedio = servico.TempoMedio;
            existente.Preco = servico.Preco;
            existente.ComissaoPercentual = servico.ComissaoPercentual;
            existente.Ativo = servico.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteServicoAsync(int id)
        {
            if (await _context.Agendamentos.AnyAsync(a => a.CodServico == id))
                throw new Exception("Não é possível excluir um serviço que possui agendamentos. Desative o cadastro.");

            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null) throw new Exception("Serviço não encontrado.");

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();
        }

        private static void Normalizar(Servicos servico)
        {
            servico.Servico = servico.Servico?.Trim().ToUpper() ?? string.Empty;
            servico.Descricao = Validacoes.Limpar(servico.Descricao);
        }

        private static void Validar(Servicos servico)
        {
            Validacoes.ValidarObrigatorio(servico.Servico, "Nome do serviço é obrigatório.");
            Validacoes.ValidarTamanho(servico.Servico, 100, "Nome do serviço");
            Validacoes.ValidarTamanho(servico.Descricao, 500, "Descrição");

            if (servico.CodTipoServico == 0)
                throw new Exception("Selecione o tipo de serviço.");

            if (servico.TempoMedio < 1 || servico.TempoMedio > 720)
                throw new Exception("Tempo médio deve ser entre 1 e 720 minutos.");

            if (servico.Preco <= 0)
                throw new Exception("Preço deve ser maior que zero.");

            if (servico.ComissaoPercentual < 0 || servico.ComissaoPercentual > 100)
                throw new Exception("Comissão deve ser entre 0% e 100%.");
        }
    }
}
