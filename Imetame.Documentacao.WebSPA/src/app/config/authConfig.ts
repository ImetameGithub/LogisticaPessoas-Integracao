import { AuthConfig } from "angular-oauth2-oidc";

export function authConfig(baseUrl: string): AuthConfig {
    return {
        // Url of the Identity Provider
        issuer: baseUrl + "/",

        // URL of the SPA to redirect the user to after login
        redirectUri: window.location.origin + "/signin-oidc",

        postLogoutRedirectUri: window.location.origin + "/index.html",

        // URL of the SPA to redirect the user after silent refresh
        silentRefreshRedirectUri:
            window.location.origin + "/silent-refresh.html",

        // The SPA's id. The SPA is registerd with this id at the auth-server
        clientId: "logistica-pessoas",

        responseType: "code",
        // set the scope for the permissions the client should request
        // The first three are defined by OIDC. The 4th is a usecase-specific one
        scope: "openid offline_access profile email imetame_api",

        // silentRefreshShowIFrame: true,

        showDebugInformation: true,

        sessionChecksEnabled: false,
        requireHttps: false,
        // timeoutFactor: 0.01,
    };
}
