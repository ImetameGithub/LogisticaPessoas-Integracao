using Imetame.Documentacao.CrossCutting.Services.Destra.Models;

namespace Imetame.Documentacao.CrossCutting.Services.Destra;

public interface IDestraService
{
    Task<AuthResponse> AuthAsync(AuthDestra authDestra);
    Task<HttpResponseMessage> GetAsync(string endPoint, string token);    
    Task<HttpResponseMessage> PostAsync(string endPoint, string json, string token);
    Task<HttpResponseMessage> GetAsyncParams(string endPoint, Dictionary<string, string> queryParams, string token);
}
