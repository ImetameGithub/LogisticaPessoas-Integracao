using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class DocumentoxColaboradorModel
    {
        public string Id { get; set; }
        public string DescArquivo { get; set; }
        public string DtVencimento { get; set; }
        public string NomeColaborador { get; set; }
        public string NomeArquivo { get; set; }
        public byte[] Bytes { get; set; } // Alterado de string para byte[]
        public string Base64 { get; set; } // Adicionado campo para armazenar a versão em base64
    }
}
