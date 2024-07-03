using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class Processamento : Entity
    {
        public Guid? Id { get; set; }
        public Guid IdPedido { get; set; }
        public string OssString { get; set; }
        public List<string> Oss { get; set; }
        public DateTime InicioProcessamento { get; set; }
        public DateTime? FinalProcessamento { get; set; }
        public StatusProcessamento Status { get; set; }
    }
}
