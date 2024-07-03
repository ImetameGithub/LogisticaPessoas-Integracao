using AutoMapper;


namespace Imetame.Documentacao.Domain.AutoMapper
{
    internal class CredenciadoraDeParaProfile : Profile
    {
        public CredenciadoraDeParaProfile()
        {
            CreateMap<Entities.CredenciadoraDePara, Models.CredenciadoraDePara>();
            CreateMap<Models.CredenciadoraDePara, Entities.CredenciadoraDePara>();
        }
    }
}