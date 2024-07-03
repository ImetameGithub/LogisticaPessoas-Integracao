using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class LogProcessamento : Entity
    {
        public LogProcessamento()
        {
            DataEvento = DateTime.Now;
        }
        public Guid IdProcessamento { get; set; }
        public Processamento Processamento { get; set; }

        public DateTime DataEvento { get; set; }
        public string Evento { get; set; }
    }
}
