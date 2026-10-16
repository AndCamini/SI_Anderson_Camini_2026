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
            tipo.Tipo = tipo.Tipo?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(tipo.Tipo, "Descrição do tipo é obrigatória.");

            var duplicado = await _context.TiposVeiculos
                .AnyAsync(t => t.Tipo == tipo.Tipo);
            if (duplicado) throw new Exception("Já existe um tipo com esse nome.");

            tipo.DataCadastro = DateTime.UtcNow;
            tipo.DataUltimaAlteracao = DateTime.UtcNow;
            tipo.UsuarioUltimaAlteracao = "sistema";

            _context.TiposVeiculos.Add(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipoAsync(TiposVeiculos tipo)
        {
            tipo.Tipo = tipo.Tipo?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(tipo.Tipo, "Descrição do tipo é obrigatória.");

            if (await _context.TiposVeiculos.AnyAsync(t => t.Tipo == tipo.Tipo && t.CodTipo != tipo.CodTipo))
                throw new Exception("Já existe um tipo com esse nome.");

            var existente = await _context.TiposVeiculos.FindAsync(tipo.CodTipo);
            if (existente == null) throw new Exception("Tipo não encontrado.");

            existente.Tipo = tipo.Tipo;
            existente.Ativo = tipo.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

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