using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
	public class DocumentoProtheus : Entity
	{
		public string IdProtheus { get; set; }
		public string DescricaoProtheus { get; set; }


		[ForeignKey("Documento")]
		public Guid IdDocumento{ get; set; }
		public virtual Documento? Documento { get; set; } = null;
	}
}
