using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Domain.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(ps =>
            {

                ps.AddProfile(new CredenciadoraDeParaProfile());
                ps.AddProfile(new PedidoProfile());
                ps.AddProfile(new ProcessamentoProfile());
            });
        }
    }
}
