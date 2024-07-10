using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class ColaboradorxAtividade : Entity
    {
        [ForeignKey("Colaborador")]
        public Guid? CXA_IDCOLABORADOR { get; set; }
        public virtual Colaborador? Colaborador { get; set; } = null;

        [ForeignKey("Atividade")]
        public Guid? CXA_IDATIVIDADE_ESPECIFICA { get; set; }
        public virtual AtividadeEspecifica? AtividadeEspecifica { get; set; } = null;
    }
}
