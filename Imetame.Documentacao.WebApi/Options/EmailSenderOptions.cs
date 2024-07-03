namespace Imetame.Documentacao.WebApi.Options
{
    public class EmailSenderOptions
    {
        public string SmtpServer { get; set; }
        public string From { get; set; }
        public int SmtpPort { get; set; }
        public bool SmtpSSL { get; set; }
        public SmtpLogin SmtpLogin { get; set; }
        public RequestPath RequestPath { get; set; }

        public string EmailDeCopiaSolicitacaoRejeitada { get; set; }
    }

    public class SmtpLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RequestPath
    {


        public string PathBase { get; set; }
        public string Host { get; set; }
        public bool IsHttps { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
    }
}
