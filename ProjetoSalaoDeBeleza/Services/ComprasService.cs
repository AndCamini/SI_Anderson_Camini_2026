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

        public async Task<Compras?> GetCompraByIdAsync(int id) =>
            await _context.Compras
                .Include(c => c.oFornecedor)
                .Include(c => c.oTransportador)
                .Include(c => c.oCondicaoPagamento)
                .Include(c => c.oFormaPagamento)
                .Include(c => c.Itens).ThenInclude(i => i.oProduto)
                .Include(c => c.Parcelas)
                .FirstOrDefaultAsync(c => c.CodCompra == id);

        public async Task AddCompraAsync(Compras compra)
        {
            Validar(compra);

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
            await _context.SaveChangesAsync();

            
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

        public async Task DeleteCompraAsync(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.Itens)
                .Include(c => c.Parcelas)
                .FirstOrDefaultAsync(c => c.CodCompra == id);

            if (compra == null) throw new Exception("Compra não encontrada.");

            
            foreach (var item in compra.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.CodProduto);
                if (produto != null)
                    produto.Estoque -= item.Quantidade;
            }

            _context.ComprasItens.RemoveRange(compra.Itens);
            _context.ComprasParcelas.RemoveRange(compra.Parcelas);
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
        }

        private void Validar(Compras compra)
        {
            if (compra.CodFornecedor == 0)
                throw new Exception("Selecione um fornecedor.");

            if (!compra.Itens.Any())
                throw new Exception("Adicione ao menos um produto.");

            if (compra.Itens.Any(i => i.Quantidade <= 0))
                throw new Exception("Todos os itens devem ter quantidade maior que zero.");

            if (compra.Itens.Any(i => i.PrecoUnitario <= 0))
                throw new Exception("Todos os itens devem ter preço unitário maior que zero.");

            if (!compra.Parcelas.Any())
                throw new Exception("Gere as parcelas antes de salvar.");
        }
    }
}