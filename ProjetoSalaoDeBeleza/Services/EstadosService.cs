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

        public async Task<List<Estados>> GetEstadosAsync() => await _context.Estados.Include(e => e.oPais).ToListAsync();
        public async Task<Estados> GetEstadoByIdAsync(int id) => await _context.Estados.FindAsync(id);
        public async Task AddEstadoAsync(Estados estado)
        {
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEstadoAsync(Estados estado)
        {
            var existente = await _context.Estados.FindAsync(estado.CodPais);
            if (existente == null) throw new Exception("Estado não encontrado.");

            existente.Estado = estado.Estado;
            existente.UF = estado.UF;
            existente.oPais = estado.oPais;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEstadoAsync(int id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado != null)
            {
                _context.Estados.Remove(estado);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Estado não encontrado.");
            }
        }
    }
}
