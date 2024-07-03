using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Imetame.Documentacao.Domain.Core.Interfaces
{
    public interface IUser
    {
        string Name { get; }

        string Cpf { get; }
        Guid UserId { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();


        string GivenName { get; }
        string Email { get; }

        List<string> Empresas { get; }

        Dictionary<string, List<string>> EmpresasComFiliais { get; }
        bool Interno { get; }

        bool PodeListarTodasAsSolicitacoes();
        bool PodeExcluirTodasAsSolicitacoes();
        bool PodeEditarTodasAsSolicitacoes();

        bool PodeReprovacoesSolicitacoesJaAprovadas();

        bool PodeEliminarResiduoDeTodasAsSolicitacoes();

        List<string> GetFiliais(string id);
        bool PodeCriarCotacaoDeTodos();
        bool PodeListarCotacaoDeTodos();

    }
}
