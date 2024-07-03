using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class LogProcessamento 
    {
        public LogProcessamento()
        {
            DataEvento = DateTime.Now;
        }
        public Guid Id { get; set; }

        public Guid IdProcessamento { get; set; }

        public DateTime DataEvento { get; set; }
        public string Evento { get; set; }
    }
}
