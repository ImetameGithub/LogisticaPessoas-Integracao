using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Infra.Data.Repositories.Common
{
    public class RestObjectsContainer<T>
    {
        public RestObjectsContainer(string empresa, string filial, IEnumerable<T> itens)
        {
            this.EMPRESA = empresa;
            this.FILIAL = filial;
            this.ITENS = itens;
        }
        public string EMPRESA { get; set; }
        public string FILIAL { get; set; }

        public string OPCAO { get; set; }

        public IEnumerable<T> ITENS { get; set; }
    }


}
