using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class MarcasVeiculosService
    {
        private readonly ApplicationDbContext _context;

        public MarcasVeiculosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MarcasVeiculos>> GetMarcasAsync() =>
            await _context.MarcasVeiculos.AsNoTracking().ToListAsync();

        public async Task AddMarcaAsync(MarcasVeiculos marca)
        {
            var duplicada = await _context.MarcasVeiculos
                .AnyAsync(m => m.MarcaVeiculo.ToLower() == marca.MarcaVeiculo.ToLower());
            if (duplicada) throw new Exception("Já existe uma marca com esse nome.");

            _context.MarcasVeiculos.Add(marca);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMarcaAsync(MarcasVeiculos marca)
        {
            var existente = await _context.MarcasVeiculos.FindAsync(marca.CodMarca);
            if (existente == null) throw new Exception("Marca não encontrada.");

            existente.MarcaVeiculo = marca.MarcaVeiculo;
            existente.Ativo = marca.Ativo;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMarcaAsync(int id)
        {
            var temVeiculos = await _context.Veiculos.AnyAsync(v => v.CodMarca == id);
            if (temVeiculos) throw new Exception("Não é possível excluir uma marca que possui veículos vinculados.");

            var marca = await _context.MarcasVeiculos.FindAsync(id);
            if (marca == null) throw new Exception("Marca não encontrada.");

            _context.MarcasVeiculos.Remove(marca);
            await _context.SaveChangesAsync();
        }
    }
}