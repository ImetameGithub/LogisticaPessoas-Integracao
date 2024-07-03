using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class PedidoList
    {
        public Guid Id { get; set; }
        public string NumPedido { get; set; }
        public string Unidade { get; set; }
        public string Credenciadora { get; set; }
    }
}
