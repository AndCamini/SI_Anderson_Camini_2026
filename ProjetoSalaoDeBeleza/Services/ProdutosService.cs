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
                .AsNoTracking()
                .Include(p => p.oCategoria)
                .ToListAsync();

        public async Task AddProdutoAsync(Produtos produto)
        {
            produto.Produto = produto.Produto?.ToUpper() ?? string.Empty;
            produto.Descricao = produto.Descricao?.ToUpper();
            produto.UnidadeMedida = produto.UnidadeMedida?.ToUpper() ?? string.Empty;

            Validar(produto);

            var duplicado = await _context.Produtos
                .AnyAsync(p => p.Produto == produto.Produto);
            if (duplicado) throw new Exception("Já existe um produto com esse nome.");

            if (produto.CodigoBarras != null &&
                await _context.Produtos.AnyAsync(p => p.CodigoBarras == produto.CodigoBarras && p.CodProduto != produto.CodProduto))
                throw new Exception("Já existe um produto com esse código de barras.");

            produto.oCategoria = null;
            produto.DataCadastro = DateTime.UtcNow;
            produto.DataUltimaAlteracao = DateTime.UtcNow;
            produto.UsuarioUltimaAlteracao = "sistema";

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProdutoAsync(Produtos produto)
        {
            produto.Produto = produto.Produto?.ToUpper() ?? string.Empty;
            produto.Descricao = produto.Descricao?.ToUpper();
            produto.UnidadeMedida = produto.UnidadeMedida?.ToUpper() ?? string.Empty;

            Validar(produto);

            var duplicado = await _context.Produtos
                .AnyAsync(p => p.Produto == produto.Produto && p.CodProduto != produto.CodProduto);
            if (duplicado) throw new Exception("Já existe um produto com esse nome.");

            if (produto.CodigoBarras != null &&
                await _context.Produtos.AnyAsync(p => p.CodigoBarras == produto.CodigoBarras && p.CodProduto != produto.CodProduto))
                throw new Exception("Já existe um produto com esse código de barras.");

            var existente = await _context.Produtos.FindAsync(produto.CodProduto);
            if (existente == null) throw new Exception("Produto não encontrado.");

            existente.Produto = produto.Produto;
            existente.Descricao = produto.Descricao;
            existente.PrecoVenda = produto.PrecoVenda;
            existente.UnidadeMedida = produto.UnidadeMedida;
            existente.Ativo = produto.Ativo;
            existente.CodCategoria = produto.CodCategoria;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProdutoAsync(int id)
        {
            if (await _context.ComprasItens.AnyAsync(i => i.CodProduto == id))
                throw new Exception("Não é possível excluir um produto que aparece em compras. Desative o cadastro.");

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) throw new Exception("Produto não encontrado.");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

        private void Validar(Produtos produto)
        {
            Validacoes.ValidarObrigatorio(produto.Produto, "Nome é obrigatório.");
            Validacoes.ValidarObrigatorio(produto.UnidadeMedida, "Unidade de medida é obrigatória.");

            if (produto.PrecoVenda <= 0)
                throw new Exception("Preço de venda deve ser maior que zero.");

            if (produto.PrecoCusto < 0)
                throw new Exception("Preço de custo não pode ser negativo.");

            if (produto.Estoque < 0)
                throw new Exception("Estoque não pode ser negativo.");

            produto.Referencia = Validacoes.Limpar(produto.Referencia)?.ToUpper();
            produto.CodigoBarras = Validacoes.Limpar(produto.CodigoBarras);
            if (produto.CodigoBarras != null && !Validacoes.CodigoBarrasValido(produto.CodigoBarras))
                throw new Exception("Código de barras inválido. Informe um EAN/GTIN de 8, 12, 13 ou 14 dígitos.");

            if (produto.PrecoVenda < produto.PrecoCusto)
                throw new Exception("Preço de venda não pode ser menor que o preço de custo.");

            if (produto.CodCategoria == 0)
                throw new Exception("Selecione uma categoria.");
        }
    }
}