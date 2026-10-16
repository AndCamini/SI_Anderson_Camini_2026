using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class CidadesService
    {
        private readonly ApplicationDbContext _context;

        public CidadesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cidades>> GetCidadesAsync() =>
            await _context.Cidades.Include(e => e.oEstado).ToListAsync();

        public async Task<Cidades> GetCidadeByIdAsync(int id) =>
            await _context.Cidades.FindAsync(id);

        public async Task AddCidadeAsync(Cidades cidade)
        {
            cidade.Cidade = cidade.Cidade?.ToUpper() ?? string.Empty;

            Validar(cidade);

            if (await _context.Cidades.AnyAsync(c => c.Cidade == cidade.Cidade && c.CodEstado == cidade.CodEstado))
                throw new Exception("Já existe uma cidade com esse nome neste estado.");

            cidade.DataCadastro = DateTime.UtcNow;
            cidade.DataUltimaAlteracao = DateTime.UtcNow;
            cidade.UsuarioUltimaAlteracao = "sistema";

            _context.Cidades.Add(cidade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCidadeAsync(Cidades cidade)
        {
            cidade.Cidade = cidade.Cidade?.ToUpper() ?? string.Empty;

            Validar(cidade);

            if (await _context.Cidades.AnyAsync(c => c.Cidade == cidade.Cidade && c.CodEstado == cidade.CodEstado && c.CodCidade != cidade.CodCidade))
                throw new Exception("Já existe uma cidade com esse nome neste estado.");

            var existente = await _context.Cidades.FindAsync(cidade.CodCidade);
            if (existente == null) throw new Exception("Cidade não encontrada.");

            existente.Cidade = cidade.Cidade;
            existente.CodEstado = cidade.CodEstado;
            existente.DDD = cidade.DDD;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCidadeAsync(int id)
        {
            if (await _context.Pessoas.AnyAsync(p => p.CodCidade == id) ||
                await _context.Fornecedores.AnyAsync(f => f.CodCidade == id) ||
                await _context.Transportadores.AnyAsync(t => t.CodCidade == id))
                throw new Exception("Não é possível excluir uma cidade vinculada a clientes, funcionários, fornecedores ou transportadores.");

            var cidade = await _context.Cidades.FindAsync(id);
            if (cidade == null) throw new Exception("Cidade não encontrada.");

            _context.Cidades.Remove(cidade);
            await _context.SaveChangesAsync();
        }

        private static void Validar(Cidades cidade)
        {
            Validacoes.ValidarObrigatorio(cidade.Cidade, "Nome da cidade é obrigatório.");

            if (cidade.CodEstado == 0)
                throw new Exception("Selecione um estado.");

            if (cidade.DDD.HasValue && (cidade.DDD < 11 || cidade.DDD > 99))
                throw new Exception("DDD inválido. Informe um valor entre 11 e 99.");
        }
    }
}