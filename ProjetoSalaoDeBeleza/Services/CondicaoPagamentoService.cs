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

        private void Validar(CondicaoPagamento condicao)
        {
            // Descrição duplicada
            var descricaoDuplicada = _context.CondicoesPagamento
                .Any(c => c.Descricao.ToLower() == condicao.Descricao.ToLower()
                       && c.CodCondicao != condicao.CodCondicao);

            if (descricaoDuplicada)
                throw new Exception("Já existe uma condição de pagamento com essa descrição.");

            // Deve ter ao menos uma parcela
            if (condicao.Parcelas == null || condicao.Parcelas.Count == 0)
                throw new Exception("A condição deve ter ao menos uma parcela.");

            // Quantidade de parcelas cadastradas deve bater com NumeroParcelas
            if (condicao.Parcelas.Count != condicao.NumeroParcelas)
                throw new Exception($"O número de parcelas informado ({condicao.NumeroParcelas}) " +
                                    $"não corresponde às parcelas cadastradas ({condicao.Parcelas.Count}).");

            // Soma dos percentuais deve ser exatamente 100%
            var totalPercentual = condicao.Parcelas.Sum(p => p.Percentual);
            if (totalPercentual != 100)
                throw new Exception($"A soma dos percentuais das parcelas deve ser 100%. " +
                                    $"Atual: {totalPercentual:F2}%.");

            // Nenhuma parcela com percentual zero ou negativo
            if (condicao.Parcelas.Any(p => p.Percentual <= 0))
                throw new Exception("Todas as parcelas devem ter percentual maior que 0%.");

            // Nenhuma parcela com dias negativos
            if (condicao.Parcelas.Any(p => p.Dias < 0))
                throw new Exception("Os dias de vencimento das parcelas não podem ser negativos.");

            // Parcelas devem estar em ordem crescente de dias
            var diasOrdenados = condicao.Parcelas.OrderBy(p => p.Numero).Select(p => p.Dias).ToList();
            for (int i = 1; i < diasOrdenados.Count; i++)
            {
                if (diasOrdenados[i] < diasOrdenados[i - 1])
                    throw new Exception($"Os dias da parcela {i + 1} devem ser maiores ou iguais " +
                                        $"aos da parcela {i}.");
            }

            // Desconto e juros não podem ser aplicados juntos (regra de negócio comum)
            if (condicao.Juros > 0 && condicao.Desconto > 0)
                throw new Exception("Não é permitido aplicar juros e desconto simultaneamente.");

            // PrimeiraParcela obrigatório se tiver mais de uma parcela
            if (condicao.NumeroParcelas > 1 && condicao.EntreParcelas == 0)
                throw new Exception("Informe o prazo entre parcelas para condições com mais de uma parcela.");
        }

        public async Task AddCondicaoAsync(CondicaoPagamento condicao)
        {
            Validar(condicao);
            _context.CondicoesPagamento.Add(condicao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCondicaoAsync(CondicaoPagamento condicao)
        {
            Validar(condicao);

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