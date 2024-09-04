namespace Imetame.Documentacao.Domain.Models;

public class ListaDocumentosDestraModel
{
	public List<DocumentosDestra> LISTA { get; set; }
}
public class DocumentosDestra
{
	public int codigo { get; set; }
	public string nome { get; set; }
	public string impeditivo { get; set; }
	public string validade { get; set; }
}

