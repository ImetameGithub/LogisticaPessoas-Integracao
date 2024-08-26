using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Pedido : Entity
    {
        public string NumPedido { get; set; }
        public string Unidade { get; set; }
        //public string Credenciadora { get; set; }

        [ForeignKey("Colaborador")]
        public Guid IdCredenciadora { get; set; }
        public virtual Credenciadora? Credenciadora { get; set; } = null;

        [NotMapped]
        public List<ColaboradorxPedido>? ColaboradorxPedido { get; set; }

    }
}
