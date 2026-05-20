using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;

namespace ProjetoSalaoDeBeleza.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ProjetoSalaoDeBeleza.Models.Paises> Paises { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Estados> Estados { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Cidades> Cidades { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Pessoas> Pessoas { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Clientes> Clientes { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Funcionarios> Funcionarios { get; set; }
    }
}
