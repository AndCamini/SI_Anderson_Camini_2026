using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

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
        public DbSet<ProjetoSalaoDeBeleza.Models.CondicaoPagamento> CondicoesPagamento { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.CondicaoPagamentoParcela> CondicoesPagamentoParcelas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pessoas>()
                .HasOne(p => p.oCidade)
                .WithMany()
                .HasForeignKey(p => p.CodCidade)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pessoas>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Pessoas>("Pessoa")
                .HasValue<Clientes>("Cliente")
                .HasValue<Funcionarios>("Funcionario");
        }
    }
}
