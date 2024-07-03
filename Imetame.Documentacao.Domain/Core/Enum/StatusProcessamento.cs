using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Core.Enum
{
    public enum StatusProcessamento
    {
        Iniciado = 0,
        Executando = 1,
        Finalizado = 2,
        FinalizadoComErro = 3
    }
}
