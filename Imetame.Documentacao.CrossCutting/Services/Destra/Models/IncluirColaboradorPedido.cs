using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.CrossCutting.Services.Destra.Models
{
    public class IncluirColaboradorPedido
    {
        public string cnpj { get; set; }
        public string numeroOS { get; set; }
        public string cpf { get; set; }
        public int[] atividadeEspecifica { get; set; }
    }
}
