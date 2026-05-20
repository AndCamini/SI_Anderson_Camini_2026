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

        public async Task<List<Funcionarios>> GetFuncionariosAsync() => await _dbContext.Funcionarios.ToListAsync();
        public async Task<Funcionarios> GetFuncionarioByIdAsync(int id) => await _dbContext.Funcionarios.FindAsync(id);
        public async Task AddFuncionarioAsync(Funcionarios funcionario)
        {
            _dbContext.Funcionarios.Add(funcionario);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateFuncionarioAsync(Funcionarios funcionario)
        {
            _dbContext.Funcionarios.Update(funcionario);
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
