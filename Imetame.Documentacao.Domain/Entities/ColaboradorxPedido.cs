using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class ColaboradorxPedido : Entity
    {
        public string CXP_NUMEROOS {  get; set; }
        public string?  CXP_USUARIOINCLUSAO {  get; set; }
        public DateTime  CXP_DTINCLUSAO {  get; set; }
        
        [ForeignKey("Colaborador")]
        public Guid? CXP_IDCOLABORADOR { get; set; }
        public virtual Colaborador? Colaborador { get; set; } = null;

        [ForeignKey("Pedido")]
        public Guid? CXP_IDPEDIDO { get; set; }
        public virtual Pedido? Pedido { get; set; } = null;
    }
}
