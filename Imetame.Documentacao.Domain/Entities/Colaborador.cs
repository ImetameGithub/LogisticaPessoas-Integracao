using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Colaborador : Entity
    {
        public string Cracha { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string MudaFuncao { get; set; }

    }
}
