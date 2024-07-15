using Imetame.Documentacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class ColaboradorxAtividadeModel
    {
        public List<ColaboradorProtheusModel> ListColaborador { get; set; }
        public List<Guid> ListAtividade { get; set; }
    }
}
