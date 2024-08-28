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
			public string Nome { get; set; }
			public string Cpf { get; set; }
            public IList<string> Atividades { get; set; }
        }
	}
}
