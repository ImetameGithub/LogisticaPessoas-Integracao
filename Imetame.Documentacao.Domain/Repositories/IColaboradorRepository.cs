using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface IColaboradorRepository
    {
        
        Task< IEnumerable<ColaboradorModel> > ListaAsync(Guid idProcessamento, CancellationToken cancellationToken);
        Task<IEnumerable<DocumentoModel>> ListaDocumentosAsync(string empresa, string numcracha, CancellationToken cancellationToken);
        Task<Domain.Models.Imagem> ObterDocumentoAsync(string empresa, long id, CancellationToken cancellationToken);
    }
}
