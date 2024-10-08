﻿using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Documento : Entity
    {
        public string Descricao { get; set; }
        public string IdDestra { get; set; }
        public string DescricaoDestra { get; set; }
        public bool Obrigatorio { get; set; } = false;

		public virtual ICollection<DocumentoXProtheus> DocumentoXProtheus { get; set; }
	}
}
