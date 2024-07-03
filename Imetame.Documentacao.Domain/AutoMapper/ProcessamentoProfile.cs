using AutoMapper;


namespace Imetame.Documentacao.Domain.AutoMapper
{
    internal class ProcessamentoProfile : Profile
    {
        public ProcessamentoProfile()
        {
            CreateMap<Entities.Processamento, Models.Processamento>();
            CreateMap<Models.Processamento, Entities.Processamento>();
        }
    }
}