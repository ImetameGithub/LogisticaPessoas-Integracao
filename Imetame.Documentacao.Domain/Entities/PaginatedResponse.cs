using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class PaginatedResponse<T>
    {
        public int totalCount { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public IList<T> data { get; set; }
    }
}
