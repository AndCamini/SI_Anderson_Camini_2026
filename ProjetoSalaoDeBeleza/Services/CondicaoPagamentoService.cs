using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class CondicaoPagamentoService
    {
        private readonly ApplicationDbContext _context;

        public CondicaoPagamentoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CondicaoPagamento>> GetCondicoesPagamentoAsync() =>
            await _context.CondicoesPagamento
                .Include(c => c.Parcelas)
                .ToListAsync();

        public async Task AddCondicaoAsync(CondicaoPagamento condicao)
        {
            _context.CondicoesPagamento.Add(condicao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCondicaoAsync(CondicaoPagamento condicao)
        {
            var existente = await _context.CondicoesPagamento
                .Include(c => c.Parcelas)
                .FirstOrDefaultAsync(c => c.CodCondicao == condicao.CodCondicao);

            if (existente == null) throw new Exception("Condição não encontrada.");

            existente.Descricao = condicao.Descricao;
            existente.NumeroParcelas = condicao.NumeroParcelas;
            existente.PrimeiraParcela = condicao.PrimeiraParcela;
            existente.EntreParcelas = condicao.EntreParcelas;
            existente.Juros = condicao.Juros;
            existente.Multa = condicao.Multa;
            existente.Desconto = condicao.Desconto;
            existente.Ativo = condicao.Ativo;

            // Remove parcelas antigas e insere as novas
            _context.CondicoesPagamentoParcelas.RemoveRange(existente.Parcelas);
            existente.Parcelas = condicao.Parcelas;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCondicaoAsync(int id)
        {
            var condicao = await _context.CondicoesPagamento
                .Include(c => c.Parcelas)
                .FirstOrDefaultAsync(c => c.CodCondicao == id);

            if (condicao == null) throw new Exception("Condição não encontrada.");

            _context.CondicoesPagamentoParcelas.RemoveRange(condicao.Parcelas);
            _context.CondicoesPagamento.Remove(condicao);
            await _context.SaveChangesAsync();
        }
    }
}