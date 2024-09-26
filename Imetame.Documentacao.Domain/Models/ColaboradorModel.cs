using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
		public bool SincronizadoDestra { get; set; } = false;
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
		public int CountDocumento { get; set; }

		// DEFINIDO COMO INT TEMPORARIAMENTE ATÉ QUE TENHA AS RELAÇÕES DO ENUM DA DESTRA
		[NotMapped]
		public int StatusDestra { get; set; } = -2;

		[NotMapped]
		public bool IsAssociado { get; set; } = false;
	}
	public class ColaboradorProtheusModel
	{
		public string MATRICULA { get; set; }
		public string CRACHA { get; set; }
		public string NOME { get; set; }
		public string NASCIMENTO { get; set; }
		public string CPF { get; set; }
		public string RG { get; set; }
		public string DATA_ADIMISSAO { get; set; }
		public string CODIGO_FUNCAO { get; set; }
		public string NOME_FUNCAO { get; set; }
		public string CODIGO_EQUIPE { get; set; }
		public string NOME_EQUIPE { get; set; }
		public string CODIGO_DISCIPLINA { get; set; }
		public string NOME_DISCIPLINA { get; set; }
		public string PERFIL { get; set; }
		public string CODIGO_OS { get; set; }
		public string NOME_OS { get; set; }
	}
}
