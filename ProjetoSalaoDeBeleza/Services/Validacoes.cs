using System.Net.Mail;
using System.Text.RegularExpressions;
using ProjetoSalaoDeBeleza.Models;

namespace ProjetoSalaoDeBeleza.Services
{
    public static class Validacoes
    {
        public static string SomenteDigitos(string? valor) =>
            new string((valor ?? string.Empty).Where(char.IsDigit).ToArray());

        public static string? Limpar(string? valor) =>
            string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

        public static bool CpfValido(string? valor)
        {
            var cpf = SomenteDigitos(valor);
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

            var soma = 0;
            for (int i = 0; i < 9; i++) soma += (cpf[i] - '0') * (10 - i);
            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;
            if (cpf[9] - '0' != digito1) return false;

            soma = 0;
            for (int i = 0; i < 10; i++) soma += (cpf[i] - '0') * (11 - i);
            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;
            return cpf[10] - '0' == digito2;
        }

        public static bool CnpjValido(string? valor)
        {
            var cnpj = SomenteDigitos(valor);
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1) return false;

            int[] pesos1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] pesos2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var soma = 0;
            for (int i = 0; i < 12; i++) soma += (cnpj[i] - '0') * pesos1[i];
            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;
            if (cnpj[12] - '0' != digito1) return false;

            soma = 0;
            for (int i = 0; i < 13; i++) soma += (cnpj[i] - '0') * pesos2[i];
            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;
            return cnpj[13] - '0' == digito2;
        }

        public static bool EmailValido(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            var email = valor.Trim();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")) return false;

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool TelefoneValido(string? valor)
        {
            var telefone = SomenteDigitos(valor);
            if (telefone.Length != 10 && telefone.Length != 11) return false;
            if (telefone[0] == '0' || telefone[1] == '0') return false;
            return telefone.Length == 10 || telefone[2] == '9';
        }

        public static bool CepValido(string? valor)
        {
            var cep = SomenteDigitos(valor);
            return cep.Length == 8 && cep.Distinct().Count() > 1;
        }

        public static bool CodigoBarrasValido(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            var codigo = valor.Trim();
            if (!codigo.All(char.IsDigit)) return false;
            if (codigo.Length is not (8 or 12 or 13 or 14)) return false;

            var soma = 0;
            for (int i = codigo.Length - 2, peso = 3; i >= 0; i--, peso = peso == 3 ? 1 : 3)
                soma += (codigo[i] - '0') * peso;

            var digito = (10 - soma % 10) % 10;
            return codigo[^1] - '0' == digito;
        }

        public static void ValidarCpf(string? valor, string campo = "CPF")
        {
            if (!CpfValido(valor)) throw new Exception($"{campo} inválido.");
        }

        public static void ValidarCnpj(string? valor, string campo = "CNPJ")
        {
            if (!CnpjValido(valor)) throw new Exception($"{campo} inválido.");
        }

        public static void ValidarEmailOpcional(string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor) && !EmailValido(valor))
                throw new Exception("E-mail inválido.");
        }

        public static void ValidarTelefoneOpcional(string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor) && !TelefoneValido(valor))
                throw new Exception("Telefone inválido. Informe DDD + número (10 ou 11 dígitos).");
        }

        public static void ValidarCepOpcional(string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor) && !CepValido(valor))
                throw new Exception("CEP inválido. Informe os 8 dígitos.");
        }

        public static void ValidarObrigatorio(string? valor, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(valor)) throw new Exception(mensagem);
        }

        public static void ValidarTamanho(string? valor, int maximo, string campo)
        {
            if (!string.IsNullOrEmpty(valor) && valor.Length > maximo)
                throw new Exception($"{campo} pode ter no máximo {maximo} caracteres.");
        }

        public static string? NormalizarEmail(string? valor)
        {
            ValidarEmailOpcional(valor);
            var email = Limpar(valor)?.ToLower();
            ValidarTamanho(email, 50, "E-mail");
            return email;
        }

        public static string? NormalizarCep(string? valor)
        {
            ValidarCepOpcional(valor);
            return string.IsNullOrWhiteSpace(valor) ? null : SomenteDigitos(valor);
        }

        public static string? NormalizarCpfOpcional(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;
            ValidarCpf(valor);
            return SomenteDigitos(valor);
        }

        public static string? NormalizarCnpjOpcional(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;
            ValidarCnpj(valor);
            return SomenteDigitos(valor);
        }

        public static void ValidarPessoa(Pessoas pessoa, bool documentoObrigatorio)
        {
            ValidarObrigatorio(pessoa.Nome, "Nome é obrigatório.");
            ValidarTamanho(pessoa.Nome, 100, "Nome");

            var documento = pessoa.PessoaJuridica ? "CNPJ" : "CPF";
            if (string.IsNullOrWhiteSpace(pessoa.CPF))
            {
                if (documentoObrigatorio) throw new Exception($"{documento} é obrigatório.");
                pessoa.CPF = string.Empty;
            }
            else
            {
                if (pessoa.PessoaJuridica) ValidarCnpj(pessoa.CPF);
                else ValidarCpf(pessoa.CPF);
                pessoa.CPF = SomenteDigitos(pessoa.CPF);
            }

            pessoa.Email = NormalizarEmail(pessoa.Email);
            ValidarTelefoneOpcional(pessoa.Telefone);
            pessoa.CEP = NormalizarCep(pessoa.CEP);

            ValidarTamanho(pessoa.Logradouro, 35, "Logradouro");
            ValidarTamanho(pessoa.Numero, 10, "Número");
            ValidarTamanho(pessoa.Complemento, 100, "Complemento");
            ValidarTamanho(pessoa.Bairro, 20, "Bairro");

            if (pessoa.DataNascimento != default)
            {
                if (pessoa.DataNascimento.Date > DateTime.Today)
                    throw new Exception("Data de nascimento não pode ser futura.");
                if (pessoa.DataNascimento.Year < 1900)
                    throw new Exception("Data de nascimento inválida.");
            }
        }
    }
}
