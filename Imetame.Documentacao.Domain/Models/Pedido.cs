using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Models
{
    public class Pedido
    {
        public Guid? Id { get; set; }
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo {0} é obrigatório")]
        public string NumPedido { get; set; }
        [StringLength(255, ErrorMessage = "O campo {0} não pode exceder {1} caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo {0} é obrigatório")]
        public string Unidade { get; set; }
        [StringLength(255, ErrorMessage = "O campo {0} não pode exceder {1} caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo {0} é obrigatório")]
        public string Credenciadora { get; set; }
    }
}
