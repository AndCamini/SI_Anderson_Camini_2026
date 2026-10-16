using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class EstadosService
    {
        private readonly ApplicationDbContext _context;

        public EstadosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estados>> GetEstadosAsync() =>
            await _context.Estados.Include(e => e.oPais).ToListAsync();

        public async Task<Estados> GetEstadoByIdAsync(int id) =>
            await _context.Estados.FindAsync(id);

        public async Task AddEstadoAsync(Estados estado)
        {
            estado.Estado = estado.Estado?.ToUpper() ?? string.Empty;
            estado.UF = estado.UF?.ToUpper() ?? string.Empty;

            Validar(estado);

            if (await _context.Estados.AnyAsync(e => e.Estado == estado.Estado))
                throw new Exception("Já existe um estado com esse nome.");

            if (await _context.Estados.AnyAsync(e => e.UF == estado.UF && e.CodPais == estado.CodPais))
                throw new Exception("Já existe um estado com essa UF neste país.");

            estado.DataCadastro = DateTime.UtcNow;
            estado.DataUltimaAlteracao = DateTime.UtcNow;
            estado.UsuarioUltimaAlteracao = "sistema";

            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEstadoAsync(Estados estado)
        {
            estado.Estado = estado.Estado?.ToUpper() ?? string.Empty;
            estado.UF = estado.UF?.ToUpper() ?? string.Empty;

            Validar(estado);

            if (await _context.Estados.AnyAsync(e => e.Estado == estado.Estado && e.CodEstado != estado.CodEstado))
                throw new Exception("Já existe um estado com esse nome.");

            if (await _context.Estados.AnyAsync(e => e.UF == estado.UF && e.CodPais == estado.CodPais && e.CodEstado != estado.CodEstado))
                throw new Exception("Já existe um estado com essa UF neste país.");

            var existente = await _context.Estados.FindAsync(estado.CodEstado);
            if (existente == null) throw new Exception("Estado não encontrado.");

            existente.Estado = estado.Estado;
            existente.UF = estado.UF;
            existente.CodPais = estado.CodPais;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEstadoAsync(int id)
        {
            if (await _context.Cidades.AnyAsync(c => c.CodEstado == id))
                throw new Exception("Não é possível excluir um estado que possui cidades vinculadas.");

            var estado = await _context.Estados.FindAsync(id);
            if (estado == null) throw new Exception("Estado não encontrado.");

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();
        }

        private static void Validar(Estados estado)
        {
            Validacoes.ValidarObrigatorio(estado.Estado, "Nome do estado é obrigatório.");

            if (estado.UF.Length < 2 || estado.UF.Length > 3 || !estado.UF.All(char.IsLetter))
                throw new Exception("UF deve ter 2 ou 3 letras.");

            if (estado.CodPais == 0)
                throw new Exception("Selecione um país.");
        }
    }
}