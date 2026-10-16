using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class TiposServicosService
    {
        private readonly ApplicationDbContext _context;

        public TiposServicosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TiposServicos>> GetTiposServicosAsync() =>
            await _context.TiposServicos.AsNoTracking().OrderBy(t => t.TipoServico).ToListAsync();

        public async Task AddTipoServicoAsync(TiposServicos tipo)
        {
            tipo.TipoServico = tipo.TipoServico?.Trim().ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(tipo.TipoServico, "Descrição do tipo é obrigatória.");

            if (await _context.TiposServicos.AnyAsync(t => t.TipoServico == tipo.TipoServico))
                throw new Exception("Já existe um tipo de serviço com essa descrição.");

            tipo.DataCadastro = DateTime.UtcNow;
            tipo.DataUltimaAlteracao = DateTime.UtcNow;
            tipo.UsuarioUltimaAlteracao = "sistema";

            _context.TiposServicos.Add(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipoServicoAsync(TiposServicos tipo)
        {
            tipo.TipoServico = tipo.TipoServico?.Trim().ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(tipo.TipoServico, "Descrição do tipo é obrigatória.");

            if (await _context.TiposServicos.AnyAsync(t => t.TipoServico == tipo.TipoServico && t.CodTipoServico != tipo.CodTipoServico))
                throw new Exception("Já existe um tipo de serviço com essa descrição.");

            var existente = await _context.TiposServicos.FindAsync(tipo.CodTipoServico);
            if (existente == null) throw new Exception("Tipo de serviço não encontrado.");

            existente.TipoServico = tipo.TipoServico;
            existente.Ativo = tipo.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTipoServicoAsync(int id)
        {
            if (await _context.Servicos.AnyAsync(s => s.CodTipoServico == id))
                throw new Exception("Não é possível excluir um tipo que possui serviços vinculados.");

            var tipo = await _context.TiposServicos.FindAsync(id);
            if (tipo == null) throw new Exception("Tipo de serviço não encontrado.");

            _context.TiposServicos.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }
}
