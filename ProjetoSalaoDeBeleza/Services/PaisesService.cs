using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public class PaisesService
    {
        private readonly ApplicationDbContext _context;

        public PaisesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Paises>> GetPaisesAsync() => await _context.Paises.ToListAsync();

        public async Task<Paises> GetPaisByIdAsync(int id) => await _context.Paises.FindAsync(id);

        public async Task AddPaisAsync(Paises pais)
        {
            pais.Pais = pais.Pais?.ToUpper() ?? string.Empty;
            pais.Sigla = pais.Sigla?.ToUpper() ?? string.Empty;
            pais.DDI = pais.DDI?.ToUpper() ?? string.Empty;
            pais.Moeda = pais.Moeda?.ToUpper() ?? string.Empty;

            Validar(pais);

            if (await _context.Paises.AnyAsync(p => p.Pais == pais.Pais))
                throw new Exception("Já existe um país com esse nome.");

            if (await _context.Paises.AnyAsync(p => p.Sigla == pais.Sigla))
                throw new Exception("Já existe um país com essa sigla.");

            pais.DataCadastro = DateTime.UtcNow;
            pais.DataUltimaAlteracao = DateTime.UtcNow;
            pais.UsuarioUltimaAlteracao = "sistema";

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaisAsync(Paises pais)
        {
            pais.Pais = pais.Pais?.ToUpper() ?? string.Empty;
            pais.Sigla = pais.Sigla?.ToUpper() ?? string.Empty;
            pais.DDI = pais.DDI?.ToUpper() ?? string.Empty;
            pais.Moeda = pais.Moeda?.ToUpper() ?? string.Empty;

            Validar(pais);

            if (await _context.Paises.AnyAsync(p => p.Pais == pais.Pais && p.CodPais != pais.CodPais))
                throw new Exception("Já existe um país com esse nome.");

            if (await _context.Paises.AnyAsync(p => p.Sigla == pais.Sigla && p.CodPais != pais.CodPais))
                throw new Exception("Já existe um país com essa sigla.");

            var existente = await _context.Paises.FindAsync(pais.CodPais);
            if (existente == null) throw new Exception("País não encontrado.");

            existente.Pais = pais.Pais;
            existente.Sigla = pais.Sigla;
            existente.DDI = pais.DDI;
            existente.Moeda = pais.Moeda;
            existente.DataUltimaAlteracao = DateTime.UtcNow;
            existente.UsuarioUltimaAlteracao = "sistema";

            await _context.SaveChangesAsync();
        }

        public async Task DeletePaisAsync(int id)
        {
            if (await _context.Estados.AnyAsync(e => e.CodPais == id))
                throw new Exception("Não é possível excluir um país que possui estados vinculados.");

            var pais = await _context.Paises.FindAsync(id);
            if (pais != null)
            {
                _context.Paises.Remove(pais);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("País não encontrado.");
            }
        }

        private static void Validar(Paises pais)
        {
            Validacoes.ValidarObrigatorio(pais.Pais, "Nome do país é obrigatório.");

            if (pais.Sigla.Length < 2 || pais.Sigla.Length > 3 || !pais.Sigla.All(char.IsLetter))
                throw new Exception("Sigla deve ter 2 ou 3 letras.");

            var ddi = pais.DDI.Trim().TrimStart('+');
            if (ddi.Length < 1 || ddi.Length > 4 || !ddi.All(char.IsDigit))
                throw new Exception("DDI inválido. Informe de 1 a 4 números (ex: 55).");

            Validacoes.ValidarObrigatorio(pais.Moeda, "Moeda é obrigatória.");
        }
    }
}