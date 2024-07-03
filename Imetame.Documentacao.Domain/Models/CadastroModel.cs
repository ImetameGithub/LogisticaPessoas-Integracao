using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class CadastroModel
    {
        public Guid IdProcessamento { get; set;}
        public IEnumerable<ColaboradorModel> Colaboradores { get; set; }


    }
}
