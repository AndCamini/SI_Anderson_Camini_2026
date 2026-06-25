using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class FornecedoresService
    {
        private readonly ApplicationDbContext _context;

        public FornecedoresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Fornecedores>> GetFornecedoresAsync() =>
            await _context.Fornecedores
                .AsNoTracking()
                .Include(f => f.oCidade)
                .ToListAsync();

        public async Task AddFornecedorAsync(Fornecedores fornecedor)
        {
            var duplicado = await _context.Fornecedores
                .AnyAsync(f => f.CNPJ == fornecedor.CNPJ);
            if (duplicado) throw new Exception("Já existe um fornecedor com este CNPJ.");

            fornecedor.oCidade = null;
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFornecedorAsync(Fornecedores fornecedor)
        {
            var duplicado = await _context.Fornecedores
                .AnyAsync(f => f.CNPJ == fornecedor.CNPJ && f.CodFornecedor != fornecedor.CodFornecedor);
            if (duplicado) throw new Exception("Já existe um fornecedor com este CNPJ.");

            var existente = await _context.Fornecedores.FindAsync(fornecedor.CodFornecedor);
            if (existente == null) throw new Exception("Fornecedor não encontrado.");

            existente.RazaoSocial = fornecedor.RazaoSocial;
            existente.NomeFantasia = fornecedor.NomeFantasia;
            existente.CNPJ = fornecedor.CNPJ;
            existente.InscricaoEstadual = fornecedor.InscricaoEstadual;
            existente.Email = fornecedor.Email;
            existente.Telefone = fornecedor.Telefone;
            existente.CEP = fornecedor.CEP;
            existente.Rua = fornecedor.Rua;
            existente.Numero = fornecedor.Numero;
            existente.Complemento = fornecedor.Complemento;
            existente.Bairro = fornecedor.Bairro;
            existente.CodCidade = fornecedor.CodCidade;
            existente.Ativo = fornecedor.Ativo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) throw new Exception("Fornecedor não encontrado.");

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }
    }
}