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
        public DbSet<ProjetoSalaoDeBeleza.Models.Categorias> Categorias { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Produtos> Produtos { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Fornecedores> Fornecedores { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Transportadores> Transportadores { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.MarcasVeiculos> MarcasVeiculos { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.TiposVeiculos> TiposVeiculos { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Veiculos> Veiculos { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.FormasPagamento> FormasPagamento { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.Compras> Compras { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.ComprasItens> ComprasItens { get; set; }
        public DbSet<ProjetoSalaoDeBeleza.Models.ComprasParcelas> ComprasParcelas { get; set; }

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

            modelBuilder.Entity<Produtos>()
                .HasOne(p => p.oCategoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CodCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fornecedores>()
                .HasOne(f => f.oCidade)
                .WithMany()
                .HasForeignKey(f => f.CodCidade)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transportadores>()
                .HasOne(t => t.oCidade)
                .WithMany()
                .HasForeignKey(t => t.CodCidade)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Veiculos>()
                .HasOne(v => v.oTransportador)
                .WithMany(t => t.Veiculos)
                .HasForeignKey(v => v.CodTransportador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Veiculos>()
                .HasOne(v => v.oMarca)
                .WithMany()
                .HasForeignKey(v => v.CodMarca)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Veiculos>()
                .HasOne(v => v.oTipo)
                .WithMany()
                .HasForeignKey(v => v.CodTipo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Clientes>()
                .HasOne(c => c.oCondicaoPagamento)
                .WithMany()
                .HasForeignKey(c => c.CodCondicaoPagamento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fornecedores>()
                .HasOne(f => f.oCondicaoPagamento)
                .WithMany()
                .HasForeignKey(f => f.CodCondicaoPagamento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Compras>()
                .HasKey(c => new { c.Modelo, c.Serie, c.NumeroNota, c.CodFornecedor });

            modelBuilder.Entity<Compras>()
                .HasOne(c => c.oFornecedor)
                .WithMany()
                .HasForeignKey(c => c.CodFornecedor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Compras>()
                .HasOne(c => c.oTransportador)
                .WithMany()
                .HasForeignKey(c => c.CodTransportador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Compras>()
                .HasOne(c => c.oCondicaoPagamento)
                .WithMany()
                .HasForeignKey(c => c.CodCondicaoPagamento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Compras>()
                .HasOne(c => c.oFormaPagamento)
                .WithMany()
                .HasForeignKey(c => c.CodFormaPagamento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComprasItens>()
                .HasOne(i => i.oCompra)
                .WithMany(c => c.Itens)
                .HasForeignKey(i => new { i.Modelo, i.Serie, i.NumeroNota, i.CodFornecedor })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComprasItens>()
                .HasOne(i => i.oProduto)
                .WithMany()
                .HasForeignKey(i => i.CodProduto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComprasParcelas>()
                .HasOne(p => p.oCompra)
                .WithMany(c => c.Parcelas)
                .HasForeignKey(p => new { p.Modelo, p.Serie, p.NumeroNota, p.CodFornecedor })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
