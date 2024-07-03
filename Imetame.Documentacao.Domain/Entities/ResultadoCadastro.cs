using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class ResultadoCadastro : Entity
    {
        public ResultadoCadastro()
        {
            Log = new List<string>();
        }
        public Guid IdProcessamento { get; set; }
        public Processamento Processamento { get; set; }
        public bool Sucesso { get; set; }
        public string NumCad { get; set; }
        public string Nome { get; set; }
        public string FuncaoAtual { get; set; }
        public string? NumCracha { get; set; } // MATHEUS FARTEC - ALTERADO PARA ACEITAR NULL DENTRO DO BANCO JÁ EXISTE VALORES COM NULL
        public string Equipe { get; set; }

        //[Column(TypeName = "nvarchar(max)")]
        public string LogString
        {
            get { return String.Join('|', Log); }
            set { Log = value.Split('|').ToList(); }
        }
        [NotMapped]
        public List<string> Log { get; set; }
    }
}
