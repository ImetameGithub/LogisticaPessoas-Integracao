using System.Text;

namespace Imetame.Documentacao.CrossCutting.Services.Protheus;

public class ProtheusService : IProtheusService
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public ProtheusService()
    {
        _httpClient = new HttpClient();
#if DEBUG 
        _url = "http://10.0.1.55:9087/rest/wslogisticapessoa";
#else
        _url = "INSERIR URL DE PRODUCAO";
#endif

    }

    public async Task<HttpResponseMessage> PostAsync(string endPoint, string json)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}{endPoint}");
            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Desafio ao chamar a API", ex);
        }
    }
}
