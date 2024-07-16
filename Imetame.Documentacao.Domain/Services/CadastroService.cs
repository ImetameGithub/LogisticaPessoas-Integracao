using DocumentFormat.OpenXml.Office2010.Excel;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Helpers;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public class CadastroService : ICadastroService
    {
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly ICadastroDestraService _cadastroDestraService;
        private readonly IProcessamentoRepository _processamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IResultadoCadastroRepository _resultadoCadastroRepository;
        private readonly IProcessamentoService _processamentoService;
        private readonly ConsoleHelper _consoleHelper;

        public CadastroService(IColaboradorRepository colaboradorRepository,
            ICadastroDestraService cadastroDestraService,
            IProcessamentoRepository processamentoRepository,
            IPedidoRepository pedidoRepository,
            IResultadoCadastroRepository resultadoCadastroRepository,
            IProcessamentoService processamentoService,
            ConsoleHelper consoleHelper)
        {
            _colaboradorRepository = colaboradorRepository;
            _cadastroDestraService = cadastroDestraService;
            _processamentoRepository = processamentoRepository;
            _pedidoRepository = pedidoRepository;
            _resultadoCadastroRepository = resultadoCadastroRepository;
            _processamentoService = processamentoService;
            _consoleHelper = consoleHelper;
        }
        // public async Task<List<Entities.ResultadoCadastro>> CadastrarAsync(CadastroModel model, CancellationToken cancellationToken)
        // {
        //     //var pedido = await _pedidoRepository.GetByIdAsync(model.IdPedido, cancellationToken);
        //     //if (pedido == null)
        //     //    throw new DomainException("Pedido não localizado");
        //     var processamento = await _processamentoRepository.GetByIdAsync(model.IdProcessamento, cancellationToken);

        //     var processamentoExistente = await _processamentoRepository.GetProcessamentoAtivo(processamento.Pedido.Id);
        //     if (processamentoExistente != null)
        //         if (processamentoExistente.Status == Core.Enum.StatusProcessamento.Executando)
        //             throw new DomainException("Existe um processamento em andamento para o pedido informado");
        //         else
        //             throw new DomainException("Processamento já foi finalizado, efetuar um novo processamento");



        //     if (model.Colaboradores == null || !model.Colaboradores.Any())
        //     {
        //         model.Colaboradores = await _colaboradorRepository.ListaAsync(model.IdProcessamento, cancellationToken);
        //     }
        //     CadastroResponse logRetorno = new CadastroResponse();

        //     string pastaTemporaria = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}");

        //     if (!Directory.Exists(pastaTemporaria))
        //     {
        //         Directory.CreateDirectory(pastaTemporaria);
        //     }

        //     try
        //     {

        //         await _processamentoService.AtualizarStatusAsync(processamento.Id, Core.Enum.StatusProcessamento.Executando, cancellationToken);
        //         _cadastroDestraService.Inicializar(processamento.Pedido.NumPedido, processamento.Id);


        //         foreach (var c in model.Colaboradores)
        //         {
        //             c.Cpf = c.Cpf.PadLeft(11, '0');
        //             DateTime start = DateTime.Now;
        //             Console.WriteLine($"[{start.ToString("dd/MM/yyyy HH:mm:ss")}] - Inicio - {c.Cpf} ************************************");
        //             Entities.ResultadoCadastro logFuncionario = new Entities.ResultadoCadastro()
        //             {
        //                 IdProcessamento = processamento.Id,
        //                 NumCad = c.NumCad,
        //                 Nome = c.Nome,
        //                 FuncaoAtual = c.FuncaoAtual,
        //                 NumCracha = c.NumCracha,
        //                 Equipe=c.Equipe
        //             };

        //             try
        //             {
        //                 //cadastrar o colaborador
        //                 _cadastroDestraService.CadastrarFuncionario(processamento.Pedido.NumPedido, c, logFuncionario, processamento.Id);
        //                 await _cadastroDestraService.AtualizarDocumentacaoAsync(processamento.Pedido.NumPedido, c, logFuncionario, pastaTemporaria, processamento.Id, cancellationToken);
        //                 logFuncionario.Sucesso = true;
        //             }
        //             catch (Exception ex)
        //             {
        //                 logFuncionario.Log.Add(ex.Message);
        //                 logFuncionario.Log.Add(ex.StackTrace);
        //                 logFuncionario.Sucesso = false;
        //                 Console.WriteLine(ex.Message);
        //                 Console.WriteLine(ex.StackTrace);
        //             }

        //             _resultadoCadastroRepository.Add(logFuncionario);
        //             await _resultadoCadastroRepository.SaveChangesAsync();
        //             logRetorno.Cadastros.Add(logFuncionario);
        //             Console.WriteLine($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - Fim - {c.Cpf} - {DateTime.Now.Subtract(start).TotalSeconds} segs ************************************");

        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _consoleHelper.Log(ex.Message, processamento.Id);
        //     }
        //     finally
        //     {
        //         _cadastroDestraService.Quit(processamento.Id);
        //         await _processamentoService.AtualizarStatusAsync(processamento.Id, Core.Enum.StatusProcessamento.Finalizado, cancellationToken);
        //         try
        //         {
        //             Directory.Delete(pastaTemporaria, true);
        //         }
        //         catch { }
        //     }


        //     return logRetorno.Cadastros;
        // }
    }
}
