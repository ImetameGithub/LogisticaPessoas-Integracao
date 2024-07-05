using Imetame.Documentacao.CrossCutting.Services.Destra.Models;

namespace Imetame.Documentacao.CrossCutting.Services.Destra;

public interface IDestraService
{
    Task<AuthResponse> AuthenticateAsync(AuthDestra authDestra);
}
