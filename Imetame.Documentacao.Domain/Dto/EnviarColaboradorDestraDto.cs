using Imetame.Documentacao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Dto
{
    public class EnviarColaboradorDestraDto
    {
        public List<ColaboradorModel> ListColaboradores { get; set; }
        public Guid IdPedido { get; set; }
        public string OrdemServico { get; set; }
        public string? MatriculaUsuario { get; set; }
    }
}
