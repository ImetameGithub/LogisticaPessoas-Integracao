using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.CrossCutting.Services.Destra.Models
{
    public class IncluirDiretaPedido
    {
        public string cnpj { get; set; }
        public string numeroOS { get; set; }
        public string tipoContrato { get; set; }
        public string pedidoCliente { get; set; }
        public string observacoes { get; set; }
        public string contatoOS { get; set; }
        public string telefoneOS { get; set; }
        
        public List<Unidade> unidades { get; set; }
        public List<Equipe> equipe { get; set; }
    }

    public class Unidade
    {
        public string cnpj { get; set; }
        public string gestor { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
    }

    public class Equipe
    {
        public string cpf { get; set; }
        public List<int> atividadeespecifica { get; set; }
    }
}
