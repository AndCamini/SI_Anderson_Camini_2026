using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public static class ComprasCalculos
    {
        public static decimal TotalProdutos(Compras compra) =>
            compra.Itens.Sum(i => i.Total);

        public static decimal TotalCustos(Compras compra) =>
            compra.Frete + compra.Seguro + compra.OutrasDespesas;

        public static decimal TotalNota(Compras compra) =>
            TotalProdutos(compra) + TotalCustos(compra);

        public static void AplicarDescontoPercentual(ComprasItens item)
        {
            item.DescontoValor = Math.Round(item.TotalBruto * item.DescontoPercentual / 100m, 2);
        }

        public static void AplicarDescontoValor(ComprasItens item)
        {
            item.DescontoPercentual = item.TotalBruto == 0
                ? 0
                : Math.Round(item.DescontoValor / item.TotalBruto * 100m, 4);
        }

        public static void RatearCustos(Compras compra)
        {
            var totalProdutos = TotalProdutos(compra);
            var custos = TotalCustos(compra);

            foreach (var item in compra.Itens)
            {
                var proporcao = totalProdutos == 0 ? 0 : item.Total / totalProdutos;
                item.CustoRateado = Math.Round(custos * proporcao, 6);
                item.CustoUnitario = item.Quantidade == 0
                    ? 0
                    : Math.Round((item.Total + item.CustoRateado) / item.Quantidade, 6);
            }
        }

        public static List<ComprasParcelas> GerarParcelas(Compras compra, CondicaoPagamento condicao)
        {
            var total = TotalNota(compra);
            var parcelasCondicao = condicao.Parcelas.OrderBy(p => p.Numero).ToList();
            var resultado = new List<ComprasParcelas>();
            decimal acumulado = 0;

            for (int i = 0; i < parcelasCondicao.Count; i++)
            {
                var parcela = parcelasCondicao[i];
                var ultima = i == parcelasCondicao.Count - 1;

                var valor = ultima
                    ? total - acumulado
                    : Math.Round(total * parcela.Percentual / 100m, 2);

                acumulado += valor;

                resultado.Add(new ComprasParcelas
                {
                    Numero = i + 1,
                    Vencimento = compra.DataEmissao.Date.AddDays(parcela.Dias),
                    Valor = valor
                });
            }

            return resultado;
        }
    }
}