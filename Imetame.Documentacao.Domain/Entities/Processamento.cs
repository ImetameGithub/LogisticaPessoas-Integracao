using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Processamento : Entity
    {
        public Processamento()
        {
            Oss = "";
        }

        public Guid IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public string OssString
        {
            get { return Oss; }
            set { Oss = value == null ? "" : value; }
        }
        [NotMapped]
        public string Oss { get; set; }

        public DateTime InicioProcessamento { get; set; }
        public DateTime? FinalProcessamento { get; set; }
        public StatusProcessamento Status { get; set; }

    }
}
