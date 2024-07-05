
using System.Net.Http.Json;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;

namespace Imetame.Documentacao.CrossCutting.Services.Destra;

public class DestraService : IDestraService
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public DestraService()
    {
        _httpClient = new HttpClient();
#if DEBUG 
        _url = "https://api.destra.armata.cloud/homolog/api/v1";
#else
        _url = "INSERIR URL DE PRODUCAO";
#endif

    }
    public async Task<AuthResponse> AuthenticateAsync(AuthDestra authDestra)
    {
        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync($"{_url}/oauth", authDestra);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Erro ao chamar a API de autenticação.", ex);
        }

        if (response.IsSuccessStatusCode)
        {
            AuthResponse authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            authResponse.Erro = false;
            return authResponse;
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new AuthResponse
            {
                Erro = true,
                MensagemErro = "Credenciais invalidas."
            };
        }
        else
        {
            return new AuthResponse
            {
                Erro = true,
                MensagemErro = $"Falha ao autenticar. Código de status: {response.StatusCode}"
            };
        }
    }
}
