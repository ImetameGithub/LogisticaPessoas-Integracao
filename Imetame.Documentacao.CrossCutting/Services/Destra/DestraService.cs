
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                _url = "https://api.destra.armata.cloud/homolog/api/v1";
        #endif

    }
    public async Task<AuthResponse> AuthAsync(AuthDestra authDestra)
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
                MensagemErro = $"Desafio ao autenticar. Código de status: {response.StatusCode}"
            };
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string endPoint, string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}{endPoint}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Desabio ao chamar a API", ex);
        }
    }

    public async Task<HttpResponseMessage> PostAsync(string endPoint, string json, string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}{endPoint}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Desafio ao chamar a API", ex);
        }
    }
}
