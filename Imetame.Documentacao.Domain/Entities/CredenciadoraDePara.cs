using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class CredenciadoraDePara:  Entity
    {
        public string Credenciadora { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
    }
}
