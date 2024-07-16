using DocumentFormat.OpenXml.Bibliography;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Helpers;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Imetame.Documentacao.Domain.Helpers.ConsoleHelper;

namespace Imetame.Documentacao.Domain.Services
{
    public class CadastroDestraService : ICadastroDestraService
    {
        private readonly IWebDriver driver;
        private readonly IColaboradorRepository _colaboradorRepository;
        private readonly ICredenciadoraDeParaRepository _credenciadoraDeParaRepository;
        private readonly Console _consoleHelper;
        private readonly ConfigDestra configDestra;

        public CadastroDestraService(IWebDriver driver, 
            IColaboradorRepository colaboradorRepository, 
            IOptions<ConfigDestra> configDestra, 
            ICredenciadoraDeParaRepository credenciadoraDeParaRepository,
            ConsoleHelper consoleHelper)
        {
            this.driver = driver;
            _colaboradorRepository = colaboradorRepository;
            this._credenciadoraDeParaRepository = credenciadoraDeParaRepository;
            this._consoleHelper = consoleHelper;
            this.configDestra = configDestra.Value;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(1000);
        }

        public void CadastrarFuncionario(string numPedido, ColaboradorModel funcionario, Entities.ResultadoCadastro logFuncionario, Guid idProcessamento)
        {
            _consoleHelper.Log($"[{funcionario.Cpf}] - CadastrarFuncionario Start", idProcessamento);
            _consoleHelper.Log("AcessarTermosLiberacao Start", idProcessamento);
            driver.AcessarTermosLiberacao(numPedido);
            _consoleHelper.Log("AcessarTermosLiberacao End", idProcessamento);

            string dadosFuncionario = "";
            bool funcionarioCadastrado = driver.VerificarFuncionarioCadastrado(funcionario.Cpf, out dadosFuncionario);
            if (funcionarioCadastrado)
            {
                _consoleHelper.Log($"[{funcionario.Cpf}] - Funcionário já cadastrado", idProcessamento);
                logFuncionario.Log.Add($"Funcionário já cadastrado");
                logFuncionario.Log.Add($"[{dadosFuncionario}");
            }
            else
            {
                _consoleHelper.Log($"[{funcionario.Cpf}] - Efetuar Cadastro", idProcessamento);
                _consoleHelper.Log($"[{funcionario.Cpf}] - CadastrarFuncionario Start", idProcessamento);
                driver.CadastrarFuncionario(funcionario, logFuncionario);
                _consoleHelper.Log($"[{funcionario.Cpf}] - CadastrarFuncionario End", idProcessamento);

            }
            _consoleHelper.Log($"[{funcionario.Cpf}] - CadastrarFuncionario End", idProcessamento);
        }


        // public async Task AtualizarDocumentacaoAsync(string numPedido, ColaboradorModel funcionario, Entities.ResultadoCadastro logFuncionario, string pasta, Guid idProcessamento, CancellationToken cancellationToken)
        // {
        //     _consoleHelper.Log($"[{funcionario.Cpf}] - AtualizarDocumentacaoAsync Start", idProcessamento);
        //     var listaDepara =await _credenciadoraDeParaRepository.ListarPorCredenciadoraAsync("Destra");
        //     //Acessar os Termos de liberacao
        //     try
        //     {
        //         _consoleHelper.Log("AcessarTermosLiberacao Start", idProcessamento);
        //         driver.AcessarTermosLiberacao(numPedido);
        //         _consoleHelper.Log("AcessarTermosLiberacao End", idProcessamento);

        //     }
        //     catch (Exception ex)
        //     {
        //         throw new DomainException($"O termo de liberação não foi localizado pelo número de pedido {numPedido}");
        //     }

        //     string dadosFuncionario = "";
        //     var cadastrado = driver.VerificarFuncionarioCadastrado(funcionario.Cpf, out dadosFuncionario);
        //     if (cadastrado)
        //     {
        //         _consoleHelper.Log($"[{funcionario.Cpf}] - GetDocumentosObrigatorios Start", idProcessamento);

        //         var documentosAtualizar = driver.GetDocumentosObrigatorios(funcionario.Cpf);
        //         _consoleHelper.Log($"[{funcionario.Cpf}] - GetDocumentosObrigatorios End", idProcessamento);

        //         var documentosBase = await _colaboradorRepository.ListaDocumentosAsync(funcionario.Empresa, funcionario.NumCracha, cancellationToken);

        //         foreach (var doc in documentosAtualizar)
        //         {

        //             if (doc.Status == "Sem documento")
        //             {
                        
        //                 var doc1 = documentosBase.FirstOrDefault(q => listaDepara.Where(d=>d.De == q.Tipo).FirstOrDefault()?.Para.ToUpper() == doc.Nome.ToUpper().Substring(1));
        //                 if (doc1 != null)
        //                 {
        //                     var imagem = await _colaboradorRepository.ObterDocumentoAsync(funcionario.Empresa, doc1.Id, cancellationToken);
        //                     string pastaTemporaria = Path.Combine(pasta, $"upload/{funcionario.Cpf}");

        //                     if (!Directory.Exists(pastaTemporaria))
        //                     {
        //                         Directory.CreateDirectory(pastaTemporaria);
        //                     }

        //                     var ext = imagem.Nome.Split('.').Last();

        //                     string pathArquivo = Path.Combine(pastaTemporaria, $"{Guid.NewGuid()}.{ext}");

                            
        //                         File.WriteAllBytes(pathArquivo, imagem.Bytes);

        //                     _consoleHelper.Log($"[{doc.Nome}] - AtualizarDocumento Start", idProcessamento);

        //                     driver.AtualizarDocumento(doc, pathArquivo, logFuncionario);
        //                     _consoleHelper.Log($"[{doc.Nome}] - AtualizarDocumento End", idProcessamento);

        //                     logFuncionario.Log.Add($"Documento atualizado {doc.Nome}");
        //                     _consoleHelper.Log($"[{funcionario.Cpf}] - Documento atualizado {doc.Nome}", idProcessamento);
                            

        //                 }
        //                 else
        //                 {
        //                     logFuncionario.Log.Add($"Documento não encontrado na Imetame {doc.Nome}");
        //                     _consoleHelper.Log($"[{funcionario.Cpf}] - Documento não encontrado na Imetame {doc.Nome}", idProcessamento);

        //                 }
        //             }
        //             else
        //             {
        //                 logFuncionario.Log.Add($"[Documento já está cadastrado {doc.Nome}");
        //                 _consoleHelper.Log($"[{funcionario.Cpf}] - Documento já está cadastrado {doc.Nome}", idProcessamento);
        //             }
        //         }

        //     }
        //     else
        //     {
        //         logFuncionario.Log.Add($"[Funcionário não está cadastrado");
        //         _consoleHelper.Log($"[{funcionario.Cpf}] - Funcionário não está cadastrado", idProcessamento);
        //     }

        //     _consoleHelper.Log($"[{funcionario.Cpf}] - AtualizarDocumentacaoAsync End", idProcessamento);


        // }

        public void Inicializar(string numPedido, Guid idProcessamento)
        {
            _consoleHelper.Log($"Inicializar Start", idProcessamento);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(10000);
            driver.Manage().Window.Maximize();
            //Login
            _consoleHelper.Log("Login Start", idProcessamento);
            driver.Login(configDestra.Usuario, configDestra.Senha);
            _consoleHelper.Log("Login End", idProcessamento);

            //Acessar os Termos de liberacao
            try
            {
                _consoleHelper.Log("AcessarTermosLiberacao Start", idProcessamento);
                driver.AcessarTermosLiberacao(numPedido);
                _consoleHelper.Log("AcessarTermosLiberacao End", idProcessamento);

            }
            catch (Exception ex)
            {
                throw new DomainException($"O termo de liberação não foi localizado pelo número de pedido {numPedido}");
            }
            _consoleHelper.Log($"Inicializar End", idProcessamento);


        }

        public void Quit(Guid idProcessamento)
        {
            _consoleHelper.Log($"Processo finalizado", idProcessamento);
            driver.Quit();
        }
    }
}
