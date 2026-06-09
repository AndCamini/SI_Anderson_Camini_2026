using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class FuncionariosService
    {
        private readonly ApplicationDbContext _dbContext;

        public FuncionariosService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Funcionarios>> GetFuncionariosAsync() =>
            await _dbContext.Funcionarios
                .Include(f => f.oCidade)
                .ToListAsync();
        public async Task<Funcionarios> GetFuncionarioByIdAsync(int id) => await _dbContext.Funcionarios.FindAsync(id);
        public async Task AddFuncionarioAsync(Funcionarios funcionario)
        {
            _dbContext.Funcionarios.Add(funcionario);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateFuncionarioAsync(Funcionarios funcionario)
        {
            var existente = await _dbContext.Funcionarios.FindAsync(funcionario.CodPessoa);
            if (existente == null) throw new Exception("Funcionário não encontrado.");

            _dbContext.Entry(existente).CurrentValues.SetValues(funcionario);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteFuncionarioAsync(int id)
        {
            var funcionario = await _dbContext.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                _dbContext.Funcionarios.Remove(funcionario);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Funcionário não encontrado.");
            }
        }
    }
}
