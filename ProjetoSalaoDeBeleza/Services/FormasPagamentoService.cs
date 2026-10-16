using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class FormasPagamentoService
    {
        private readonly ApplicationDbContext _context;

        public FormasPagamentoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormasPagamento>> GetFormasPagamentoAsync() =>
            await _context.FormasPagamento.AsNoTracking().ToListAsync();

        public async Task AddFormaPagamentoAsync(FormasPagamento forma)
        {
            forma.FormaPagamento = forma.FormaPagamento?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(forma.FormaPagamento, "Descrição da forma de pagamento é obrigatória.");

            var duplicada = await _context.FormasPagamento
                .AnyAsync(f => f.FormaPagamento == forma.FormaPagamento);
            if (duplicada) throw new Exception("Já existe uma forma de pagamento com essa descrição.");

            forma.DataCadastro = DateTime.UtcNow;
            forma.DataUltimaAlteracao = DateTime.UtcNow;
            forma.UsuarioUltimaAlteracao = "sistema";

            _context.FormasPagamento.Add(forma);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFormaPagamentoAsync(FormasPagamento forma)
        {
            forma.FormaPagamento = forma.FormaPagamento?.ToUpper() ?? string.Empty;

            Validacoes.ValidarObrigatorio(forma.FormaPagamento, "Descrição da forma de pagamento é obrigatória.");

            var duplicada = await _context.FormasPagamento
                .AnyAsync(f => f.FormaPagamento == forma.FormaPagamento
                            && f.CodFormaPagamento != forma.CodFormaPagamento);
            if (duplicada) throw new Exception("Já existe uma forma de pagamento com essa descrição.");

            var existente = await _context.FormasPagamento.FindAsync(forma.CodFormaPagamento);
            if (existente == null) throw new Exception("Forma de pagamento não encontrada.");

            existente.FormaPagamento = forma.FormaPagamento;
            existente.Ativo = forma.Ativo;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteFormaPagamentoAsync(int id)
        {
            if (await _context.Compras.AnyAsync(c => c.CodFormaPagamento == id))
                throw new Exception("Não é possível excluir uma forma de pagamento usada em compras. Desative o cadastro.");

            var forma = await _context.FormasPagamento.FindAsync(id);
            if (forma == null) throw new Exception("Forma de pagamento não encontrada.");

            _context.FormasPagamento.Remove(forma);
            await _context.SaveChangesAsync();
        }
    }
}