﻿using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Entities
{
	public class AtividadeEspecifica : Entity
	{
		public string Codigo { get; set; }
		public string Descricao { get; set; }
		public int IdDestra { get; set; }
		
		[NotMapped]
		public int QuantColaboradores { get; set; } = 0;

	}
}
