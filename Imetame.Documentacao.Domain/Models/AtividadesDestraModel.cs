using Imetame.Documentacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class ListaAtividadesModel
    {
        public List<AtividadeDestra> LISTA { get; set; }
    }

    public class AtividadeDestra
    {
        public string codigo { get; set; }
        public int id { get; set; }
        public string descricao { get; set; }
    }

}
