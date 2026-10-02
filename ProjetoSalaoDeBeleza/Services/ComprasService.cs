using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class ComprasService
    {
        private readonly ApplicationDbContext _context;

        public ComprasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Compras>> GetComprasAsync() =>
            await _context.Compras
                .AsNoTracking()
                .Include(c => c.oFornecedor)
                .Include(c => c.oTransportador)
                .Include(c => c.oCondicaoPagamento)
                .Include(c => c.oFormaPagamento)
                .Include(c => c.Itens).ThenInclude(i => i.oProduto)
                .Include(c => c.Parcelas)
                .ToListAsync();

        public async Task AddCompraAsync(Compras compra)
        {
            Validar(compra);

            var duplicada = await _context.Compras.AnyAsync(c =>
                c.Modelo == compra.Modelo &&
                c.Serie == compra.Serie &&
                c.NumeroNota == compra.NumeroNota &&
                c.CodFornecedor == compra.CodFornecedor);

            if (duplicada)
                throw new Exception("Esta nota já foi lançada para este fornecedor.");

            compra.Observacoes = compra.Observacoes?.ToUpper();
            compra.DataCadastro = DateTime.UtcNow;
            compra.DataUltimaAlteracao = DateTime.UtcNow;
            compra.UsuarioUltimaAlteracao = "sistema";
            compra.oFornecedor = null;
            compra.oTransportador = null;
            compra.oCondicaoPagamento = null;
            compra.oFormaPagamento = null;
            foreach (var item in compra.Itens) item.oProduto = null;

            _context.Compras.Add(compra);

            foreach (var item in compra.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.CodProduto);
                if (produto != null)
                {
                    produto.Estoque += item.Quantidade;
                    produto.PrecoCusto = item.PrecoUnitario;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompraAsync(int modelo, int serie, int numeroNota, int codFornecedor)
        {
            var compra = await _context.Compras
                .Include(c => c.Itens)
                .Include(c => c.Parcelas)
                .FirstOrDefaultAsync(c =>
                    c.Modelo == modelo &&
                    c.Serie == serie &&
                    c.NumeroNota == numeroNota &&
                    c.CodFornecedor == codFornecedor);

            if (compra == null) throw new Exception("Compra não encontrada.");

            foreach (var item in compra.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.CodProduto);
                if (produto != null)
                    produto.Estoque -= item.Quantidade;
            }

            // Itens e parcelas saem junto pelo cascade
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
        }

        private void Validar(Compras compra)
        {
            if (compra.Modelo <= 0)
                throw new Exception("Informe o modelo da nota.");

            if (compra.Serie <= 0)
                throw new Exception("Informe a série da nota.");

            if (compra.NumeroNota <= 0)
                throw new Exception("Informe o número da nota.");

            if (compra.CodFornecedor == 0)
                throw new Exception("Selecione um fornecedor.");

            if (!compra.Itens.Any())
                throw new Exception("Adicione ao menos um produto.");

            if (compra.Itens.Any(i => i.CodProduto == 0))
                throw new Exception("Selecione o produto em todas as linhas.");

            if (compra.Itens.Any(i => i.Quantidade <= 0))
                throw new Exception("Todos os itens devem ter quantidade maior que zero.");

            if (compra.Itens.Any(i => i.PrecoUnitario <= 0))
                throw new Exception("Todos os itens devem ter preço unitário maior que zero.");

            if (!compra.Parcelas.Any())
                throw new Exception("Gere as parcelas antes de salvar.");
        }
    }
}