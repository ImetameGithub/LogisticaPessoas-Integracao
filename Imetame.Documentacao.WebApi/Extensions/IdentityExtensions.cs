

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Imetame.Documentacao.WebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            //System.Security.Claims.ClaimTypes.
            var id = principal.FindFirst("sub")?.Value;

            return id;
        }

        public static List<string> GetUserEmpresas(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            //System.Security.Claims.ClaimTypes.
            var empresas = principal.FindFirst("empresas")?.Value;
            if (string.IsNullOrEmpty(empresas))
                return new List<string>();

            var ef = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(empresas);
            if (ef == null)
                return new List<string>();

            return new List<string>(ef.Keys);

        }

        public static Dictionary<string, List<string>> GetUserEmpresasComFiliais(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            //System.Security.Claims.ClaimTypes.
            var empresas = principal.FindFirst("empresas")?.Value;
            if (string.IsNullOrEmpty(empresas))
                return new Dictionary<string, List<string>>();

            var ef = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(empresas);
            if (ef == null)
                return new Dictionary<string, List<string>>();

            return ef;

        }





        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            var id = principal.FindFirst("email")?.Value;

            return id;
        }

        public static string GetUserGivenName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            var id = principal.FindFirst("given_name")?.Value ?? "";

            return id;
        }

        public static string GetUserCpf(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            //OpenIdConnectConstants.Claims.Subject
            var id = principal.FindFirst("cpf")?.Value ?? "";

            return id;
        }


        //public static bool HasClaim(this ClaimsPrincipal principal, string claimType, string claimValyue)
        //{
        //    if (principal == null)
        //        throw new ArgumentNullException(nameof(principal));
        //    //OpenIdConnectConstants.Claims.Subject
        //    //System.Security.Claims.ClaimTypes.
        //    var id = principal.FindFirst("sub")?.Value;

        //    return id;
        //}
    }
}
