using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Documento : Entity
    {
        public string Descricao { get; set; }
        public string IdDestra { get; set; }
        public string DescricaoDestra { get; set; }
        public bool Obrigatorio { get; set; } = false;

        public virtual ICollection<DocumentoXProtheus> DocumentoXProtheus { get; set; }
    }

    public class DocumentoStatus
    {
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        [JsonPropertyName("statusDescricao")]
        public string StatusDescricao { get; set; }

        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        public string CodProtheus { get; set; }
    }
}
