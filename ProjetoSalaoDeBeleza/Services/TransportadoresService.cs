using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class TransportadoresService
    {
        private readonly ApplicationDbContext _context;

        public TransportadoresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transportadores>> GetTransportadoresAsync() =>
            await _context.Transportadores
                .AsNoTracking()
                .Include(t => t.oCidade)
                .ToListAsync();

        public async Task AddTransportadorAsync(Transportadores transportador)
        {
            if (string.IsNullOrWhiteSpace(transportador.CPF) && string.IsNullOrWhiteSpace(transportador.CNPJ))
                throw new Exception("Informe CPF ou CNPJ.");

            transportador.oCidade = null;
            _context.Transportadores.Add(transportador);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransportadorAsync(Transportadores transportador)
        {
            if (string.IsNullOrWhiteSpace(transportador.CPF) && string.IsNullOrWhiteSpace(transportador.CNPJ))
                throw new Exception("Informe CPF ou CNPJ.");

            var existente = await _context.Transportadores.FindAsync(transportador.CodTransportador);
            if (existente == null) throw new Exception("Transportador não encontrado.");

            existente.Nome = transportador.Nome;
            existente.CPF = transportador.CPF;
            existente.CNPJ = transportador.CNPJ;
            existente.Email = transportador.Email;
            existente.Telefone = transportador.Telefone;
            existente.CEP = transportador.CEP;
            existente.Rua = transportador.Rua;
            existente.Numero = transportador.Numero;
            existente.Complemento = transportador.Complemento;
            existente.Bairro = transportador.Bairro;
            existente.CodCidade = transportador.CodCidade;
            existente.Ativo = transportador.Ativo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransportadorAsync(int id)
        {
            var temVeiculos = await _context.Veiculos.AnyAsync(v => v.CodTransportador == id);
            if (temVeiculos) throw new Exception("Não é possível excluir um transportador que possui veículos vinculados.");

            var transportador = await _context.Transportadores.FindAsync(id);
            if (transportador == null) throw new Exception("Transportador não encontrado.");

            _context.Transportadores.Remove(transportador);
            await _context.SaveChangesAsync();
        }
    }
}