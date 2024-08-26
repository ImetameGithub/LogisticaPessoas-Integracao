using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace Imetame.Documentacao.CrossCutting.Services.Destra.Models
{
    public class ColaboradorDestraApiModel
	{
        public int CODERR { get; set; }
        public string iss { get; set; }
        public int uid { get; set; }
        public string HOJE { get; set; }
		public IList<ColaboradorDestra> DADOS { get; set; } 
	}

	public class ColaboradorDestra
	{
		public string? nome { get; set; }
		public string? nascto { get; set; }
		public string? cpf { get; set; }
		public string? rg { get; set; }
		public string? passaporte { get; set; }
		public string? passaporteValidade { get; set; }
		public string? cnh { get; set; }
		public string? cnhCategoria { get; set; }
		public string? CnhValidade { get; set; }
		public string? crea { get; set; }
		public string? creaUF { get; set; }
		public int? idCidade { get; set; }
		public string? cnpj { get; set; }
		public string? funcao { get; set; }
		public int? idVinculo { get; set; }
		public string? dataAdmissao { get; set; }
		public string? isTemporario { get; set; }
		public int? contratoDias { get; set; }
		public string? contratoFim { get; set; }
		public string? salarioTipo { get; set; }
		public decimal? salarioValor { get; set; }
		public int? status { get; set; }
		public string? statusDescricao { get; set; }
		public int? statusVinculo { get; set; }
		public string? nomeCidade { get; set; }
	}
}
