using AutoMapper;


namespace Imetame.Documentacao.Domain.AutoMapper
{
    internal class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Entities.Pedido, Models.Pedido>();
            CreateMap<Models.Pedido, Entities.Pedido>();
        }
    }
}