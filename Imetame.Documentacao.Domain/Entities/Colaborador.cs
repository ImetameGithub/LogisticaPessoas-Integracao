using Imetame.Documentacao.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Imetame.Documentacao.Domain.Entities
{
    public class Colaborador : Entity
    {
        public string Matricula { get; set; }
        public string Cracha { get; set; }
        public string Nome { get; set; }
        public string MudaFuncao { get; set; }
        public string Codigo_Funcao { get; set; }
        public string Nome_Funcao { get; set; }
        public string Codigo_Equipe { get; set; }    
        public string Nome_Disciplina { get; set; }
        public string Codigo_Disciplina { get; set; }
        public string Nome_Equipe { get; set; }
        public string Perfil { get; set; }
        public string Codigo_OS { get; set; }
        public string Nome_OS { get; set; }

        //[JsonIgnore]
        [NotMapped]
        public List<ColaboradorxAtividade> ColaboradorxAtividade { get; set; }

    }
}
