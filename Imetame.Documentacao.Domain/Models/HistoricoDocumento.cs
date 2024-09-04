using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
	public class HistoricoDocumento
	{
		public class HistoricoDestraApiModel
		{
			public int? CODERR { get; set; }
			public string? iss { get; set; }
			public int? uid { get; set; }
			public string? HOJE { get; set; }
			public int? QTDE { get; set; }
			public List<HistoricoDocumentoDestra>? LISTA { get; set; }
		}

		public class HistoricoDocumentoDestra
		{
			public int? pagina { get; set; }
			public string? dtUpload { get; set; }
			public int? tamanho { get; set; }
			public string? impeditivo { get; set; }
			public string? nomeDocto { get; set; }
			public string? cpf { get; set; }
			public string? arquivo { get; set; }
			public int? id { get; set; }
			public int? idDocto { get; set; }
			public string? validade { get; set; }
			public int? status { get; set; }
		}
	}
}
