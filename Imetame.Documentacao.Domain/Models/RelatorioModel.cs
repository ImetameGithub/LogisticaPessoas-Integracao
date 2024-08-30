using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
	public class RelatorioModel
	{

		public class ChecklistModel
		{
			public string? Nome { get; set; }
			public string? Matricula { get; set; }
			public string? Equipe { get; set; }
			public string? DataAdmissao { get; set; }
			public string? OrdemServico { get; set; }
			public string? NumPedido { get; set; }
			public IList<string>? ItensDestra { get; set; }
			//public string? DataVencimento { get; set; } = "Pendente";
			public int? StatusDestra { get; set; }
			public string? Rg { get; set; }
			public string? Ctps { get; set; }
			public string? Cpf { get; set; }
			public IList<string>? Atividades { get; set; }
		}
	}
}
