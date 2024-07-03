using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class ColaboradorModel
    {
        public string Empresa { get; set; }
        public string NumCad { get; set; }
        public string NumCracha { get; set; }
        public string Status { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string FuncaoAtual { get; set; }
        public string FuncaoInicial { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Equipe { get; set; }
        public string Perfil { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Ddd { get; set; }
        public string Numtel { get; set; }
        public string Estado { get; set; }
        public string TempoEmpresaAnos { get; set; }
        public string TempoEmpresaAnosInt { get; set; }
    }
}
