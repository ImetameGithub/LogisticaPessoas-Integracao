using Imetame.Documentacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class CadastroResponse
    {
        public CadastroResponse()
        {
            Cadastros = new List<Entities.ResultadoCadastro>();
        }
        public List<Entities.ResultadoCadastro> Cadastros { get; set; }
    }
   
}
