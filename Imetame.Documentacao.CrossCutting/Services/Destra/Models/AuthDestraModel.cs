namespace Imetame.Documentacao.CrossCutting.Services.Destra.Models
{
    public class AuthDestra
    {
        public string Login { get; set; }
        public string Pwd { get; set; }

    }

    public class AuthResponse
    {        
        public bool Erro { get; set; }
        public string MensagemErro { get; set; }
        public string Token { get; set; }
    }
}
