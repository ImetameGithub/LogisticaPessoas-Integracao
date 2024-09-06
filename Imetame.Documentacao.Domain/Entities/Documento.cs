using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Documento : Entity
    {
        public string Descricao { get; set; }
        public string IdDocCredenciadora { get; set; } // Guarda o id do documento na credenciadora ex: 146 é o id do cpf na destra
        public string DescricaoCredenciadora { get; set; } // Descricao do documento na credenciadora
        public string IdProtheus { get; set; }
        public string DescricaoProtheus { get; set; }
        public bool Obrigatorio { get; set; } = false;


		[ForeignKey("Credenciadora")]
		public Guid IdCredenciadora { get; set; }
		public virtual Credenciadora? Credenciadora { get; set; } = null;
	}
}
