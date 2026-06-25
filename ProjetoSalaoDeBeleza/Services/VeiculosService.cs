using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class VeiculosService
    {
        private readonly ApplicationDbContext _context;

        public VeiculosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Veiculos>> GetVeiculosAsync() =>
            await _context.Veiculos
                .AsNoTracking()
                .Include(v => v.oMarca)
                .Include(v => v.oTipo)
                .Include(v => v.oTransportador)
                .ToListAsync();

        public async Task AddVeiculoAsync(Veiculos veiculo)
        {
            Validar(veiculo);

            var duplicado = await _context.Veiculos
                .AnyAsync(v => v.Placa.ToUpper() == veiculo.Placa.ToUpper());
            if (duplicado) throw new Exception("Já existe um veículo com esta placa.");

            veiculo.Placa = veiculo.Placa.ToUpper();
            veiculo.oMarca = null;
            veiculo.oTipo = null;
            veiculo.oTransportador = null;
            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVeiculoAsync(Veiculos veiculo)
        {
            Validar(veiculo);

            var duplicado = await _context.Veiculos
                .AnyAsync(v => v.Placa.ToUpper() == veiculo.Placa.ToUpper()
                            && v.CodVeiculo != veiculo.CodVeiculo);
            if (duplicado) throw new Exception("Já existe um veículo com esta placa.");

            var existente = await _context.Veiculos.FindAsync(veiculo.CodVeiculo);
            if (existente == null) throw new Exception("Veículo não encontrado.");

            existente.Placa = veiculo.Placa.ToUpper();
            existente.PlacaMercosul = veiculo.PlacaMercosul;
            existente.Modelo = veiculo.Modelo;
            existente.Cor = veiculo.Cor;
            existente.Ano = veiculo.Ano;
            existente.CodMarca = veiculo.CodMarca;
            existente.CodTipo = veiculo.CodTipo;
            existente.CodTransportador = veiculo.CodTransportador;
            existente.Ativo = veiculo.Ativo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteVeiculoAsync(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) throw new Exception("Veículo não encontrado.");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
        }

        private void Validar(Veiculos veiculo)
        {
            if (string.IsNullOrWhiteSpace(veiculo.Placa))
                throw new Exception("Placa é obrigatória.");

            if (veiculo.PlacaMercosul)
            {
                // Formato Mercosul: AAA0A00 (3 letras, 1 número, 1 letra, 2 números)
                var placa = veiculo.Placa.ToUpper().Trim();
                if (placa.Length != 7)
                    throw new Exception("Placa Mercosul deve ter exatamente 7 caracteres (ex: ABC1D23).");

                bool formatoValido =
                    char.IsLetter(placa[0]) &&
                    char.IsLetter(placa[1]) &&
                    char.IsLetter(placa[2]) &&
                    char.IsDigit(placa[3]) &&
                    char.IsLetter(placa[4]) &&
                    char.IsDigit(placa[5]) &&
                    char.IsDigit(placa[6]);

                if (!formatoValido)
                    throw new Exception("Formato de placa Mercosul inválido. Use o formato ABC1D23 (3 letras, 1 número, 1 letra, 2 números).");
            }
            else
            {
                // Formato antigo: AAA0000 (3 letras, 4 números)
                var placa = veiculo.Placa.ToUpper().Trim();
                if (placa.Length != 7)
                    throw new Exception("Placa deve ter exatamente 7 caracteres (ex: ABC1234).");

                bool formatoValido =
                    char.IsLetter(placa[0]) &&
                    char.IsLetter(placa[1]) &&
                    char.IsLetter(placa[2]) &&
                    char.IsDigit(placa[3]) &&
                    char.IsDigit(placa[4]) &&
                    char.IsDigit(placa[5]) &&
                    char.IsDigit(placa[6]);

                if (!formatoValido)
                    throw new Exception("Formato de placa inválido. Use o formato ABC1234 (3 letras e 4 números).");
            }

            if (veiculo.CodMarca == 0) throw new Exception("Selecione uma marca.");
            if (veiculo.CodTipo == 0) throw new Exception("Selecione um tipo.");
            if (veiculo.CodTransportador == 0) throw new Exception("Selecione um transportador.");
            if (veiculo.Ano < 1900 || veiculo.Ano > DateTime.Now.Year + 1)
                throw new Exception($"Ano inválido. Informe um valor entre 1900 e {DateTime.Now.Year + 1}.");
        }
    }
}