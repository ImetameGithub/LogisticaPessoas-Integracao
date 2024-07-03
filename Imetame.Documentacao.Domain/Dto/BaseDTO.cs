using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Domain.Dto
{
    public abstract class BaseDTO
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
    }
}
