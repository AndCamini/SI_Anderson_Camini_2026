using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class CategoriasService
    {
        private readonly ApplicationDbContext _context;

        public CategoriasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categorias>> GetCategoriasAsync() =>
            await _context.Categorias.ToListAsync();

        public async Task AddCategoriaAsync(Categorias categoria)
        {
            categoria.Categoria = categoria.Categoria?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(categoria.Categoria, "Nome da categoria é obrigatório.");

            var duplicada = await _context.Categorias
                .AnyAsync(c => c.Categoria == categoria.Categoria);
            if (duplicada)
                throw new Exception("Já existe uma categoria com esse nome.");

            categoria.DataCadastro = DateTime.UtcNow;
            categoria.DataUltimaAlteracao = DateTime.UtcNow;
            categoria.UsuarioUltimaAlteracao = "sistema";

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoriaAsync(Categorias categoria)
        {
            categoria.Categoria = categoria.Categoria?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(categoria.Categoria, "Nome da categoria é obrigatório.");

            var duplicada = await _context.Categorias
                .AnyAsync(c => c.Categoria == categoria.Categoria
                            && c.CodCategoria != categoria.CodCategoria);
            if (duplicada)
                throw new Exception("Já existe uma categoria com esse nome.");

            var existente = await _context.Categorias.FindAsync(categoria.CodCategoria);
            if (existente == null) throw new Exception("Categoria não encontrada.");

            existente.Categoria = categoria.Categoria;
            existente.Ativo = categoria.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var temProdutos = await _context.Produtos.AnyAsync(p => p.CodCategoria == id);
            if (temProdutos)
                throw new Exception("Não é possível excluir uma categoria que possui produtos vinculados.");

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) throw new Exception("Categoria não encontrada.");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}