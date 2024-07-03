using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public interface ICadastroDestraService
    {
        void Inicializar(string numPedido, Guid idProcessamento);
        void CadastrarFuncionario(string numPedido, ColaboradorModel funcionario, Entities.ResultadoCadastro logFuncionario, Guid idProcessamento);
        Task AtualizarDocumentacaoAsync(string numPedido, ColaboradorModel funcionario, Entities.ResultadoCadastro logFuncionario,string pasta, Guid idProcessamento, CancellationToken cancellationToken);
        void Quit(Guid idProcessamento);
    }
}
