using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class DocumentoModel
    {
        public long Id { get; set; }
        public string Tipo { get; set; }
        public string Sequencia { get; set; }
        public string Nome { get; set; }
    }
}
