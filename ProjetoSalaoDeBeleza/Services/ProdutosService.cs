using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class ProdutosService
    {
        private readonly ApplicationDbContext _context;

        public ProdutosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produtos>> GetProdutosAsync() =>
            await _context.Produtos
                .Include(p => p.oCategoria)
                .ToListAsync();

        public async Task AddProdutoAsync(Produtos produto)
        {
            Validar(produto);

            var duplicado = await _context.Produtos
                .AnyAsync(p => p.Produto.ToLower() == produto.Produto.ToLower());

            if (duplicado)
                throw new Exception("Já existe um produto com esse nome.");

            produto.oCategoria = null;
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProdutoAsync(Produtos produto)
        {
            Validar(produto);

            var duplicado = await _context.Produtos
                .AnyAsync(p => p.Produto.ToLower() == produto.Produto.ToLower()
                            && p.CodProduto != produto.CodProduto);

            if (duplicado)
                throw new Exception("Já existe um produto com esse nome.");

            var existente = await _context.Produtos.FindAsync(produto.CodProduto);
            if (existente == null) throw new Exception("Produto não encontrado.");

            existente.Produto = produto.Produto;
            existente.Descricao = produto.Descricao;
            existente.PrecoCusto = produto.PrecoCusto;
            existente.PrecoVenda = produto.PrecoVenda;
            existente.Estoque = produto.Estoque;
            existente.UnidadeMedida = produto.UnidadeMedida;
            existente.Ativo = produto.Ativo;
            existente.CodCategoria = produto.CodCategoria;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) throw new Exception("Produto não encontrado.");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        private void Validar(Produtos produto)
        {
            if (produto.PrecoVenda < produto.PrecoCusto)
                throw new Exception("Preço de venda não pode ser menor que o preço de custo.");

            if (produto.CodCategoria == 0)
                throw new Exception("Selecione uma categoria.");
        }
    }
}