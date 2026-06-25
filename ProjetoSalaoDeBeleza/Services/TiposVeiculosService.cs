using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class TiposVeiculoService
    {
        private readonly ApplicationDbContext _context;

        public TiposVeiculoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TiposVeiculos>> GetTiposAsync() =>
            await _context.TiposVeiculos.AsNoTracking().ToListAsync();

        public async Task AddTipoAsync(TiposVeiculos tipo)
        {
            var duplicado = await _context.TiposVeiculos
                .AnyAsync(t => t.Tipo.ToLower() == tipo.Tipo.ToLower());
            if (duplicado) throw new Exception("Já existe um tipo com esse nome.");

            _context.TiposVeiculos.Add(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipoAsync(TiposVeiculos tipo)
        {
            var existente = await _context.TiposVeiculos.FindAsync(tipo.CodTipo);
            if (existente == null) throw new Exception("Tipo não encontrado.");

            existente.Tipo = tipo.Tipo;
            existente.Ativo = tipo.Ativo;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTipoAsync(int id)
        {
            var temVeiculos = await _context.Veiculos.AnyAsync(v => v.CodTipo == id);
            if (temVeiculos) throw new Exception("Não é possível excluir um tipo que possui veículos vinculados.");

            var tipo = await _context.TiposVeiculos.FindAsync(id);
            if (tipo == null) throw new Exception("Tipo não encontrado.");

            _context.TiposVeiculos.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }
}