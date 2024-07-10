namespace Imetame.Documentacao.CrossCutting.Services.Protheus;

public interface IProtheusService
{
    Task<HttpResponseMessage> PostAsync(string endPoint, string json);
}
