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
                .OrderByDescending(c => c.DataEmissao)
                .ToListAsync();

        public async Task<bool> ExisteCompraAsync(int modelo, int serie, int numeroNota, int codFornecedor) =>
            await _context.Compras.AnyAsync(c =>
                c.Modelo == modelo &&
                c.Serie == serie &&
                c.NumeroNota == numeroNota &&
                c.CodFornecedor == codFornecedor);

        public async Task AddCompraAsync(Compras compraTela)
        {
            Validar(compraTela);

            var duplicada = await _context.Compras.AnyAsync(c =>
                c.Modelo == compraTela.Modelo &&
                c.Serie == compraTela.Serie &&
                c.NumeroNota == compraTela.NumeroNota &&
                c.CodFornecedor == compraTela.CodFornecedor);

            if (duplicada)
                throw new Exception("Esta nota já foi lançada para este fornecedor.");

            // Recalcula no servidor para não depender do que veio da tela
            ComprasCalculos.RatearCustos(compraTela);

            var totalParcelas = compraTela.Parcelas.Sum(p => p.Valor);
            if (Math.Abs(totalParcelas - ComprasCalculos.TotalNota(compraTela)) > 0.01m)
                throw new Exception("A soma das parcelas não confere com o total da nota. Gere as parcelas novamente.");

            // Monta um objeto novo, sem as navegações, para o EF não tentar inserir
            // fornecedor/produto de novo e para não alterar os objetos da tela
            var compra = new Compras
            {
                Modelo = compraTela.Modelo,
                Serie = compraTela.Serie,
                NumeroNota = compraTela.NumeroNota,
                CodFornecedor = compraTela.CodFornecedor,
                DataEmissao = compraTela.DataEmissao.Date,
                DataChegada = compraTela.DataChegada.Date,
                CodTransportador = compraTela.CodTransportador,
                Frete = compraTela.Frete,
                Seguro = compraTela.Seguro,
                OutrasDespesas = compraTela.OutrasDespesas,
                CodCondicaoPagamento = compraTela.CodCondicaoPagamento,
                CodFormaPagamento = compraTela.CodFormaPagamento,
                Observacoes = compraTela.Observacoes?.ToUpper(),
                DataCadastro = DateTime.UtcNow,
                DataUltimaAlteracao = DateTime.UtcNow,
                UsuarioUltimaAlteracao = "sistema",
                Itens = compraTela.Itens.Select(i => new ComprasItens
                {
                    CodProduto = i.CodProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    DescontoPercentual = i.DescontoPercentual,
                    DescontoValor = i.DescontoValor,
                    CustoRateado = i.CustoRateado,
                    CustoUnitario = i.CustoUnitario
                }).ToList(),
                Parcelas = compraTela.Parcelas.Select(p => new ComprasParcelas
                {
                    Numero = p.Numero,
                    Vencimento = p.Vencimento.Date,
                    Valor = p.Valor
                }).ToList()
            };

            _context.Compras.Add(compra);

            // Entrada no estoque e atualização do custo (já com frete rateado)
            foreach (var item in compra.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.CodProduto);
                if (produto == null)
                    throw new Exception($"Produto {item.CodProduto} não encontrado.");

                produto.Estoque += item.Quantidade;
                produto.PrecoCusto = item.CustoUnitario;
                produto.DataUltimaAlteracao = DateTime.UtcNow;
                produto.UsuarioUltimaAlteracao = "sistema";
            }

            // Um único SaveChanges: compra e estoque gravam juntos ou nada é gravado
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

            _context.ComprasItens.RemoveRange(compra.Itens);
            _context.ComprasParcelas.RemoveRange(compra.Parcelas);
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
        }

        private static void Validar(Compras compra)
        {
            if (compra.Modelo <= 0) throw new Exception("Informe o modelo da nota.");
            if (compra.Serie <= 0) throw new Exception("Informe a série da nota.");

            if (compra.NumeroNota <= 0 || compra.NumeroNota > 999999999)
                throw new Exception("Número da nota deve ter entre 1 e 9 dígitos.");

            if (compra.CodFornecedor == 0) throw new Exception("Selecione um fornecedor.");

            if (compra.DataEmissao.Date > DateTime.Today)
                throw new Exception("Data de emissão não pode ser posterior a hoje.");

            if (compra.DataChegada.Date < compra.DataEmissao.Date)
                throw new Exception("Data de chegada não pode ser anterior à data de emissão.");

            if (compra.DataChegada.Date > DateTime.Today)
                throw new Exception("Data de chegada não pode ser posterior a hoje.");

            if (!compra.Itens.Any()) throw new Exception("Adicione ao menos um produto.");

            if (compra.Itens.Any(i => i.CodProduto == 0))
                throw new Exception("Selecione o produto em todas as linhas.");

            if (compra.Itens.Any(i => i.Quantidade <= 0))
                throw new Exception("Todos os itens devem ter quantidade maior que zero.");

            if (compra.Itens.Any(i => i.PrecoUnitario <= 0))
                throw new Exception("Todos os itens devem ter valor unitário maior que zero.");

            if (compra.Itens.Any(i => i.DescontoValor < 0 || i.DescontoValor > i.TotalBruto))
                throw new Exception("O desconto não pode ser negativo nem maior que o total do item.");

            if (compra.Frete < 0 || compra.Seguro < 0 || compra.OutrasDespesas < 0)
                throw new Exception("Frete, seguro e outras despesas não podem ser negativos.");

            if (compra.CodCondicaoPagamento == null)
                throw new Exception("Selecione a condição de pagamento.");

            if (!compra.Parcelas.Any())
                throw new Exception("Gere as parcelas antes de salvar.");
        }
    }
}