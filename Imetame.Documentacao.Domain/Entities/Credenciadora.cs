using DocumentFormat.OpenXml.Math;
using Imetame.Documentacao.Domain.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Imetame.Documentacao.Domain.Entities;

public class Credenciadora : Entity
{
    public string Descricao { get; set; }

    [NotMapped]    
    public List<Pedido> Pedidos { get; set; }
}
