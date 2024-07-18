using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class DocumentoxColaborador : Entity
    {
        public string DXC_CODPROTHEUS { get; set; }
        public string DXC_CODDESTRA { get; set; }
        public string DXC_BASE64 { get; set; }


        [ForeignKey("Colaborador")]
        public Guid DXC_IDCOLABORADOR { get; set; }
        public virtual Colaborador Colaborador { get; set; }
    }
}
