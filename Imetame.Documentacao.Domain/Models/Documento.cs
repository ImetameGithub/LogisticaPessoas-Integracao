using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class Documento
    {
        public string IdRef { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public string Observacao { get; set; }
        public string Adicionais { get; set; }
        public DateTime? Validade { get; set; }
        public string UrlUpload { get; set; }
    }
}
