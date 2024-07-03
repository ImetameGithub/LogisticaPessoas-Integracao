
using Imetame.Documentacao.WebApi.Extensions;
using Imetame.Documentacao.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Imetame.Documentacao.WebApi.Helpers
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public bool Interno => IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("interno", "true") : false;

        public Guid UserId => IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

        public List<string> Empresas => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmpresas() : new List<string>();

        public Dictionary<string, List<string>> EmpresasComFiliais => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmpresasComFiliais() : new Dictionary<string, List<string>>();

        public string GivenName => IsAuthenticated() ? _accessor.HttpContext.User.GetUserGivenName() : string.Empty;

        public string Cpf => IsAuthenticated() ? _accessor.HttpContext.User.GetUserCpf() : string.Empty;

        public string Email => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;



        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }



        public List<string> GetFiliais(string empresa)
        {
            if (IsAuthenticated())
            {
                var ef = _accessor.HttpContext.User.GetUserEmpresasComFiliais();
                if (ef.ContainsKey(empresa))
                    return ef[empresa];
            }


            return new List<string>();
        }

        public Dictionary<string, List<string>> GetUserEmpresasComFiliais()
        {
            if (IsAuthenticated())
            {
                return _accessor.HttpContext.User.GetUserEmpresasComFiliais();

            }


            return new Dictionary<string, List<string>>();
        }


        public bool PodeListarTodasAsSolicitacoes()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("solicitacao_de_compras", "listar_todas") : false;
        }

        public bool PodeReprovacoesSolicitacoesJaAprovadas()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("aprovacao", "reprovar_ja_aprovado") : false;
        }

        public bool PodeExcluirTodasAsSolicitacoes()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("solicitacao_de_compras", "excluir_todas") : false;
        }

        public bool PodeEditarTodasAsSolicitacoes()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("solicitacao_de_compras", "editar_todas") : false;
        }

        public bool PodeEliminarResiduoDeTodasAsSolicitacoes()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("solicitacao_de_compras", "eliminar_residuo_todas") : false;
        }

        public bool PodeCriarCotacaoDeTodos()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("cotacao", "criar_todos") : false;
        }

        public bool PodeListarCotacaoDeTodos()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.HasClaim("cotacao", "listar_todas") : false;
        }
    }
}
