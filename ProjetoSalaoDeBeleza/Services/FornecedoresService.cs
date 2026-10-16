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
            fornecedor.RazaoSocial = fornecedor.RazaoSocial?.ToUpper() ?? string.Empty;
            fornecedor.NomeFantasia = fornecedor.NomeFantasia?.ToUpper();
            fornecedor.CNPJ = fornecedor.CNPJ?.ToUpper() ?? string.Empty;
            fornecedor.InscricaoEstadual = fornecedor.InscricaoEstadual?.ToUpper();
            fornecedor.Rua = fornecedor.Rua?.ToUpper();
            fornecedor.Bairro = fornecedor.Bairro?.ToUpper();
            fornecedor.Complemento = fornecedor.Complemento?.ToUpper();

            Validar(fornecedor);

            var duplicado = await _context.Fornecedores
                .AnyAsync(f => f.CNPJ == fornecedor.CNPJ);
            if (duplicado) throw new Exception("Já existe um fornecedor com este CNPJ.");

            if (fornecedor.CodCidade == 0)
                throw new Exception("Selecione uma cidade.");

            fornecedor.oCidade = null;
            fornecedor.DataCadastro = DateTime.UtcNow;
            fornecedor.DataUltimaAlteracao = DateTime.UtcNow;
            fornecedor.UsuarioUltimaAlteracao = "sistema";

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFornecedorAsync(Fornecedores fornecedor)
        {
            fornecedor.RazaoSocial = fornecedor.RazaoSocial?.ToUpper() ?? string.Empty;
            fornecedor.NomeFantasia = fornecedor.NomeFantasia?.ToUpper();
            fornecedor.CNPJ = fornecedor.CNPJ?.ToUpper() ?? string.Empty;
            fornecedor.InscricaoEstadual = fornecedor.InscricaoEstadual?.ToUpper();
            fornecedor.Rua = fornecedor.Rua?.ToUpper();
            fornecedor.Bairro = fornecedor.Bairro?.ToUpper();
            fornecedor.Complemento = fornecedor.Complemento?.ToUpper();

            Validar(fornecedor);

            var duplicado = await _context.Fornecedores
                .AnyAsync(f => f.CNPJ == fornecedor.CNPJ && f.CodFornecedor != fornecedor.CodFornecedor);
            if (duplicado) throw new Exception("Já existe um fornecedor com este CNPJ.");

            var existente = await _context.Fornecedores.FindAsync(fornecedor.CodFornecedor);
            if (existente == null) throw new Exception("Fornecedor não encontrado.");

            if (fornecedor.CodCidade == 0)
                throw new Exception("Selecione uma cidade.");

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
            existente.CodCondicaoPagamento = fornecedor.CodCondicaoPagamento;
            existente.Ativo = fornecedor.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFornecedorAsync(int id)
        {
            if (await _context.Compras.AnyAsync(c => c.CodFornecedor == id))
                throw new Exception("Não é possível excluir um fornecedor que possui compras lançadas. Desative o cadastro.");

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) throw new Exception("Fornecedor não encontrado.");

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }

        private static void Validar(Fornecedores fornecedor)
        {
            Validacoes.ValidarObrigatorio(fornecedor.RazaoSocial, "Razão social é obrigatória.");
            Validacoes.ValidarObrigatorio(fornecedor.CNPJ, "CNPJ é obrigatório.");
            Validacoes.ValidarCnpj(fornecedor.CNPJ);
            fornecedor.CNPJ = Validacoes.SomenteDigitos(fornecedor.CNPJ);

            fornecedor.Email = Validacoes.NormalizarEmail(fornecedor.Email);
            Validacoes.ValidarTelefoneOpcional(fornecedor.Telefone);
            fornecedor.CEP = Validacoes.NormalizarCep(fornecedor.CEP);

            if (string.IsNullOrWhiteSpace(fornecedor.InscricaoEstadual))
            {
                fornecedor.InscricaoEstadual = null;
                return;
            }

            var ie = fornecedor.InscricaoEstadual.Trim().ToUpper();
            if (ie == "ISENTO") return;

            var digitos = Validacoes.SomenteDigitos(ie);
            if (ie.Any(c => !char.IsDigit(c) && c != '.' && c != '-' && c != '/') || digitos.Length < 8 || digitos.Length > 14)
                throw new Exception("Inscrição estadual inválida. Informe de 8 a 14 números ou ISENTO.");
            fornecedor.InscricaoEstadual = digitos;
        }
    }
}