using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class ClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Clientes>> GetClientesAsync() =>
            await _context.Clientes
                .Include(c => c.oCidade)
                .ToListAsync();

        public async Task<Clientes> GetClienteByIdAsync(int id) =>
            await _context.Clientes
                .Include(c => c.oCidade)
                .FirstOrDefaultAsync(c => c.CodPessoa == id);

        public async Task AddClienteAsync(Clientes cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(Clientes cliente)
        {
            var existente = await _context.Clientes.FindAsync(cliente.CodPessoa);
            if (existente == null) throw new Exception("Cliente não encontrado.");

            _context.Entry(existente).CurrentValues.SetValues(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cliente não encontrado.");
            }
        }
    }
}