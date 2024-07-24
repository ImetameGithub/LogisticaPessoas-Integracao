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
        public string Codigo { get; set; }
        public string DescArquivo { get; set; }
        public string DtVencimento { get; set; }
        public DateTime DtVencimentoFormatada { get; set; }
        public string NomeColaborador { get; set; }
        public string Matricula { get; set; }
        public string NomeArquivo { get; set; }
        public byte[] Bytes { get; set; } // Alterado de string para byte[]
        public string Base64 { get; set; } // Adicionado campo para armazenar a versão em base64
        public bool SincronizadoDestra { get; set; } = false;



        // REGRAS PARA VALIDAÇÃO DE DOCUMENTOS 
        public bool Vencido { get; set; } = false;
        public bool Vencer { get; set; } = false;
        public int DiasVencer { get; set; }

    }
}
