using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class DocumentoxColaboradorModel
    {
        //public string Id { get; set; }
        public string Codigo { get; set; }
        public string DescArquivo { get; set; }
        public string DtVencimento { get; set; }
        public DateTime DtVencimentoFormatada { get; set; }
        public string NomeColaborador { get; set; }
        public string Matricula { get; set; }
        public string NomeArquivo { get; set; }
        public string Recno { get; set; } // Adicionado campo para armazenar a versão em base64
        public byte[]? Bytes { get; set; }
        public string? Base64 { get; set; }
        public bool SincronizadoDestra { get; set; } = false;
        public string ConcatAtividades { get; set; } = "";



        // REGRAS PARA VALIDAÇÃO DE DOCUMENTOS 
        public bool Vencido { get; set; } = false;
        public bool Vencer { get; set; } = false;
        public int DiasVencer { get; set; }

    }

    // DESSA MANEIRA PARA QUE EU POSSAR RECEBER UM OBJETO NO ANGULAR E PASSAR PARA A CLASSE ACIMA - ASSIM NÃO ALTERO O CIRCUITO JÁ FEITO PARA ENVIO DE DOCS
    public class ImagemProtheus
    {
        public byte[] Bytes { get; set; }
        public string Base64 { get; set; }
    }
}
