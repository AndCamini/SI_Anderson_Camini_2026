using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class ClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Clientes>> GetClientesAsync() =>
            await _context.Clientes
                .AsNoTracking()
                .Include(c => c.oCidade)
                .ToListAsync();

        public async Task<Clientes> GetClienteByIdAsync(int id) =>
            await _context.Clientes
                .Include(c => c.oCidade)
                .FirstOrDefaultAsync(c => c.CodPessoa == id);

        public async Task AddClienteAsync(Clientes cliente)
        {
            cliente.Nome = cliente.Nome?.ToUpper() ?? string.Empty;
            cliente.CPF = cliente.CPF?.ToUpper() ?? string.Empty;
            cliente.Logradouro = cliente.Logradouro?.ToUpper();
            cliente.Bairro = cliente.Bairro?.ToUpper();
            cliente.Complemento = cliente.Complemento?.ToUpper();

            Validacoes.ValidarPessoa(cliente, false);

            if (!string.IsNullOrWhiteSpace(cliente.CPF) &&
                await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF))
                throw new Exception("Já existe um cliente com este CPF/CNPJ.");

            if (!string.IsNullOrWhiteSpace(cliente.Email) &&
                await _context.Clientes.AnyAsync(c => c.Email == cliente.Email))
                throw new Exception("Já existe um cliente com este e-mail.");

            cliente.oCidade = null;
            cliente.DataCadastro = DateTime.UtcNow;
            cliente.DataUltimaAlteracao = DateTime.UtcNow;
            cliente.UsuarioUltimaAlteracao = "sistema";

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(Clientes cliente)
        {
            cliente.Nome = cliente.Nome?.ToUpper() ?? string.Empty;
            cliente.CPF = cliente.CPF?.ToUpper() ?? string.Empty;
            cliente.Logradouro = cliente.Logradouro?.ToUpper();
            cliente.Bairro = cliente.Bairro?.ToUpper();
            cliente.Complemento = cliente.Complemento?.ToUpper();

            Validacoes.ValidarPessoa(cliente, false);

            if (!string.IsNullOrWhiteSpace(cliente.CPF) &&
                await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF && c.CodPessoa != cliente.CodPessoa))
                throw new Exception("Já existe um cliente com este CPF/CNPJ.");

            if (!string.IsNullOrWhiteSpace(cliente.Email) &&
                await _context.Clientes.AnyAsync(c => c.Email == cliente.Email && c.CodPessoa != cliente.CodPessoa))
                throw new Exception("Já existe um cliente com este e-mail.");

            var existente = await _context.Clientes.FindAsync(cliente.CodPessoa);
            if (existente == null) throw new Exception("Cliente não encontrado.");

            existente.Nome = cliente.Nome;
            existente.CPF = cliente.CPF;
            existente.Email = cliente.Email;
            existente.Telefone = cliente.Telefone;
            existente.DataNascimento = cliente.DataNascimento;
            existente.CodCidade = cliente.CodCidade;
            existente.Logradouro = cliente.Logradouro;
            existente.Bairro = cliente.Bairro;
            existente.Complemento = cliente.Complemento;
            existente.Numero = cliente.Numero;
            existente.CEP = cliente.CEP;
            existente.Observacoes = cliente.Observacoes;
            existente.RecebeNotificacoes = cliente.RecebeNotificacoes;
            existente.Ativo = cliente.Ativo;
            existente.PessoaJuridica = cliente.PessoaJuridica;
            existente.CodCondicaoPagamento = cliente.CodCondicaoPagamento;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            if (await _context.Agendamentos.AnyAsync(a => a.CodCliente == id))
                throw new Exception("Não é possível excluir um cliente que possui agendamentos. Desative o cadastro.");

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) throw new Exception("Cliente não encontrado.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}