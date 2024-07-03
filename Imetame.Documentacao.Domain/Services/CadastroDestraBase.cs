using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Helpers;
using Imetame.Documentacao.Domain.Models;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Console = Imetame.Documentacao.Domain.Helpers.ConsoleHelper;

namespace Imetame.Documentacao.Domain.Services
{
    public static class CadastroDestraBase
    {
        public static void Login(this IWebDriver driver, string usuario, string senha)
        {
            driver.Navigate().GoToUrl("https://servicelayer.destra.net.br/destra/");

            var textBoxemail = driver.FindElement(By.Name("email"));
            var textBoxpass = driver.FindElement(By.Name("pass"));
            var submitButton = driver.FindElement(By.TagName("button"));

            textBoxemail.SendKeys(usuario);
            textBoxpass.SendKeys(senha);
            submitButton.Click();
        }

        public static void AcessarTermosLiberacao(this IWebDriver driver, string numPedido)
        {
          

            //Navegacao incial
            driver.Navigate().GoToUrl("https://servicelayer.destra.net.br/destra/termoLiberacao/fornecedor/filter.do");

            driver.FindElement(By.Name("numPedido")).SendKeys(numPedido);
            driver.FindElement(By.XPath("//*[@id=\"page-wrapper\"]/div/div[3]/div/div/div[3]/button[1]")).Click();

            driver.FindElement(By.XPath("//*[@id=\"table-documento\"]/tbody/tr/td[7]/a")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));

        }

        #region ColetarFuncionarios
        public static List<ColaboradorModel> ColetarFuncionarios(this IWebDriver driver)
        {

            List<ColaboradorModel> funcionariosCadastrados = new List<ColaboradorModel>();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement selectElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("table-funcionarios_length")));
            SelectElement select = new SelectElement(selectElement);
            select.SelectByValue("100");

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));

            IWebElement tabela01 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("table-funcionarios")));
            TratarRegistrosFuncionarios(tabela01, funcionariosCadastrados);

            // Se houver paginação, navegue para a próxima página e repita o processo
            while (TableHelper.ExisteProximaPagina(driver, "table-funcionarios_next"))
            {
                TableHelper.IrParaProximaPagina(driver, "table-funcionarios_next");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));

                IWebElement tabelaN = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("table-funcionarios")));
                TratarRegistrosFuncionarios(tabelaN, funcionariosCadastrados);
            }


            return funcionariosCadastrados;

        }
        public static void TratarRegistrosFuncionarios(IWebElement tabela, List<ColaboradorModel> funcionariosCadastrados)
        {

            IList<IWebElement> registros = tabela.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
            foreach (var registro in registros)
            {
                IList<IWebElement> colunas = registro.FindElements(By.TagName("td"));

                ColaboradorModel funcionario = new ColaboradorModel();
                funcionario.Nome = colunas[0].Text;
                funcionario.DataNascimento = colunas[1].Text == string.Empty ? DateTime.MinValue : DateTime.Parse(colunas[1].Text);
                funcionario.Cpf = colunas[2].Text;
                funcionario.Estado = colunas[3].Text;
                funcionario.Cidade = colunas[4].Text;
                funcionario.FuncaoAtual = colunas[5].Text;
                //funcionario.Atividades = colunas[6].Text;
                //funcionario.Conforme = colunas[7].FindElements(By.TagName("a"))[1].GetAttribute("title") != "Existe documento que não está em conformidade.";

                funcionariosCadastrados.Add(funcionario);

            }
        }

        public static bool VerificarFuncionarioCadastrado(this IWebDriver driver, string cpf, out string dadosFuncionario)
        {

            var trFuncionario = driver.PesquisaFuncionarioCadastrado(cpf);
            dadosFuncionario = trFuncionario.Text;
            return trFuncionario.FindElements(By.TagName("td")).Count > 1;
        }

        /// <summary>
        /// Retorna a tr do funcionario
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static IWebElement PesquisaFuncionarioCadastrado(this IWebDriver driver, string cpf)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));
            var searchInput = driver.FindElement(By.XPath("//*[@id=\"table-funcionarios_filter\"]/label/input"));
            searchInput.Clear();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));
            searchInput.SendKeys(cpf);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-funcionarios_processing")));
            Thread.Sleep(1000);
            IWebElement tabela01 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("table-funcionarios")));

            return tabela01.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[0];
        }


        #endregion

        public static void CadastrarFuncionario(this IWebDriver driver, ColaboradorModel funcionario, Entities.ResultadoCadastro logRetorno)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            driver.FindElement(By.XPath("//*[@id=\"funcionariosLoad\"]/div[1]/div/button")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("qtdInserir")));

            driver.FindElement(By.Name("qtdInserir")).Clear();
            driver.FindElement(By.Name("qtdInserir")).SendKeys("1");
            driver.FindElement(By.XPath("//*[@id=\"modal-pre-inserir\"]/div[3]/button[2]")).Click();

            Thread.Sleep(2000);
            IWebElement tabelaCadastro = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"frmFuncionarios\"]/table")));
            IList<IWebElement> registros = tabelaCadastro.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

            IList<IWebElement> colunas = registros[0].FindElements(By.TagName("td"));
            try
            {
                colunas[0].FindElement(By.Name($"funCPF")).SendKeys(funcionario.Cpf);
                Thread.Sleep(500);
                colunas[1].FindElement(By.Name("funNome")).Click();
            }
            catch (UnhandledAlertException ex)
            {
                logRetorno.Log.Add($"Cadastro com erro {ex.AlertText}");
                driver.FindElement(By.XPath("//*[@id=\"modal-funcionario\"]/div[4]/button[1]")).Click();
                return;
            }


            colunas[1].FindElement(By.Name("funNome")).SendKeys(funcionario.Nome);
            Thread.Sleep(500);
            colunas[2].FindElement(By.Name("funDtNascto")).Click();
            colunas[2].FindElement(By.Name("funDtNascto")).SendKeys(funcionario.DataNascimento.ToString("ddMMyyyy"));
            Thread.Sleep(500);
            colunas[3].FindElement(By.Name("funUF")).SelectValue(funcionario.Estado);
            Thread.Sleep(1000);
            colunas[4].FindElement(By.Name("funCidade")).SelectValue(funcionario.Cidade);
            Thread.Sleep(500);
            colunas[5].FindElement(By.Name("funFuncao")).SendKeys(funcionario.FuncaoAtual);
            Thread.Sleep(500);

            colunas[6].FindElement(By.TagName("button")).Click();
            colunas[6].FindElement(By.XPath("//span/div/ul/li[3]")).Click();
            colunas[6].FindElement(By.TagName("button")).Click();


            Thread.Sleep(1000);



            //driver.FindElement(By.XPath("//*[@id=\"modal-funcionario\"]/div[4]/button[1]")).Click();
            //var button = driver.FindElement(By.Id("btnFuncGrava"));
            //button.Click();
            driver.ExecuteJavaScript("TERMOLIBERACAO.salvarListaFuncionarios()");
            
            logRetorno.Log.Add($"Preenchimento OK");

            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
            if (alert.Text == "Funcionários inseridos com sucesso.")
                logRetorno.Log.Add($"Cadastrado com sucesso");
            else
                logRetorno.Log.Add($"[Cadastro com erro {alert.Text}");
            alert.Accept();

        }


        #region AtualizarDocumentacao
        public static List<Documento> GetDocumentosObrigatorios(this IWebDriver driver, string cpfFuncionario)
        {

            var trFuncionario = driver.PesquisaFuncionarioCadastrado(cpfFuncionario);
            var paramsFuncionario = trFuncionario.FindElement(By.XPath("//td[8]/a[2]")).GetAttribute("href").Split('?')[1];
            driver.Navigate().GoToUrl(trFuncionario.FindElement(By.XPath("//td[8]/a[2]")).GetAttribute("href"));

            List<Documento> documentos = new List<Documento>();

            //Navegar nos documentos
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement selectElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("table-documento_length")));
            SelectElement select = new SelectElement(selectElement);
            select.SelectByValue("100");

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-documento_processing")));

            IWebElement tabelaDocumento = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("table-documento")));

            TratarRegistrosDocumentos(tabelaDocumento, paramsFuncionario, documentos);

            // Se houver paginação, navegue para a próxima página e repita o processo
            while (TableHelper.ExisteProximaPagina(driver, "table-documento_next"))
            {
                TableHelper.IrParaProximaPagina(driver, "table-documento_next");
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.Id("table-documento_processing")));

                IWebElement tabelaN = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("table-documento")));
                TratarRegistrosDocumentos(tabelaDocumento, paramsFuncionario, documentos);
            }

            return documentos.Where(d => d.Nome.StartsWith("*")).ToList();

        }
        public static void TratarRegistrosDocumentos(IWebElement tabela, string paramsFuncionario, List<Documento> documentos)
        {
            IList<IWebElement> registros = tabela.FindElements(By.TagName("tr"));
            foreach (var registro in registros)
            {
                IList<IWebElement> colunas = registro.FindElements(By.TagName("td"));
                if (colunas.Count > 0)
                {
                    Documento documento = new Documento();
                    documento.Nome = colunas[0].Text;
                    documento.Status = colunas[1].Text;
                    documento.Observacao = colunas[2].Text;
                    documento.Adicionais = colunas[3].Text;
                    documento.Validade = colunas[4].Text == string.Empty ? null : DateTime.Parse(colunas[4].Text.Substring(0, 10));
                    documento.IdRef = GetParametrosFuncao("upload", colunas[5].FindElements(By.TagName("a"))[0].GetAttribute("href"))[0];
                    documento.UrlUpload = $"https://servicelayer.destra.net.br/destra/termoLiberacao/funcionario/documento/upload.do?cldoId={documento.IdRef}&{paramsFuncionario}";
                    documentos.Add(documento);
                }
            }
        }
        public static bool AtualizarDocumento(this IWebDriver driver, Documento documento, string arquivo, Entities.ResultadoCadastro logFuncionario)
        {

            try
            {
                driver.Navigate().GoToUrl(documento.UrlUpload);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement fileElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("doc_file")));

                fileElement.SendKeys(arquivo);
                if (driver.FindElements(By.Id("dataValidade")).Count > 0)
                    driver.FindElement(By.Id("dataValidade"))?.SendKeys(DateTime.Now.AddDays(60).ToString("ddMMyyyy"));

                if (driver.FindElements(By.Name("valorCampoEspecifico")).Count > 0)
                    driver.FindElement(By.Name("valorCampoEspecifico"))?.SendKeys("12");



                driver.FindElement(By.XPath("//*[@id=\"page-wrapper\"]/div/div[3]/div/div/div[3]/button[2]")).Click();
                Thread.Sleep(1000);
            }
            catch (UnhandledAlertException ex)
            {
                logFuncionario.Log.Add($"[{documento.Nome}] - Upload com erro {ex.AlertText}");
                return false;
            }
            

            return true;
        }

        private static string[] GetParametrosFuncao(string funcao, string input)
        {
            string pattern = @$"{funcao}\(([^)]*)\);";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                string parametros = match.Groups[1].Value;
                // Divida os parâmetros usando a vírgula como delimitador
                string[] parametrosArray = parametros.Split(',');
                return parametrosArray;
            }
            return new string[0];
        }
        #endregion
    }
}
