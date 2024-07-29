namespace Imetame.Documentacao.CrossCutting.Services.Destra.Models
{
    public class DocumentoDestra
    {    
        public string cpf { get; set; }
        public string idDocto { get; set; }
        public string validade { get; set; }        
        public byte[] arquivo { get; set; } // Alterado de string para byte[]        
        public string pagina { get; set; }        
    }
}
