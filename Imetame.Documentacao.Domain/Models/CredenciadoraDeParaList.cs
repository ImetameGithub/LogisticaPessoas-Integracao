using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class CredenciadoraDeParaList
    {
        public Guid Id { get; set; }
        public string Credenciadora { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
    }
}
