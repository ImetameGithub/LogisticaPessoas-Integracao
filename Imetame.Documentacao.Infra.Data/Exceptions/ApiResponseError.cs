using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Infra.Data.Exceptions

{
    public class ApiResponseError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
