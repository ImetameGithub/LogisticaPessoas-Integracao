using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imetame.Documentacao.WebApi.Options
{
    public class IdentityOptions
    {
        //
        // Summary:
        //     Gets or sets the absolute URL of the OAuth2/OpenID Connect server. Note: this
        //     property is ignored when AspNet.Security.OAuth.Introspection.OAuthIntrospectionOptions.Configuration
        //     or AspNet.Security.OAuth.Introspection.OAuthIntrospectionOptions.ConfigurationManager
        //     are set.
        public Uri Authority { get; set; }
        //
        // Summary:
        //     Gets or sets the client secret used to communicate with the introspection endpoint.
        public string ClientSecret { get; set; }
        //
        // Summary:
        //     Gets or sets the client identifier representing the resource server.
        public string ClientId { get; set; }
        //
        // Summary:
        //     Gets the intended audiences of this resource server. Setting this property is
        //     recommended when the authorization server issues access tokens for multiple distinct
        //     resource servers.
        public string Audience { get; set; }
        //
        // Summary:
        //     Gets or sets a boolean indicating whether HTTPS is required to retrieve the metadata
        //     document. The default value is true. This option should be used only in development
        //     environments. Note: this property is ignored when AspNet.Security.OAuth.Introspection.OAuthIntrospectionOptions.Configuration
        //     or AspNet.Security.OAuth.Introspection.OAuthIntrospectionOptions.ConfigurationManager
        //     are set.
        public bool RequireHttpsMetadata { get; set; }
    }
}
