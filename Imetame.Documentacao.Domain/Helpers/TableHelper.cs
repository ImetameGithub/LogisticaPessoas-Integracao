using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Imetame.Documentacao.Domain.Helpers
{
    public static class TableHelper
    {

        public static bool ExisteProximaPagina(IWebDriver driver, string idButtonNext)
        {
            try
            {
                // Verifique se há um botão ou elemento que representa a próxima página
                IWebElement proximaPagina = driver.FindElement(By.Id(idButtonNext));
                string classes = proximaPagina.GetAttribute("class");

                return proximaPagina.Displayed && proximaPagina.Enabled && !classes.Contains("disabled");
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void IrParaProximaPagina(IWebDriver driver, string idButtonNext)
        {
            // Encontre e clique no botão da próxima página
            IWebElement proximaPagina = driver.FindElement(By.Id(idButtonNext));
            proximaPagina.Click();
        }
    }
}
