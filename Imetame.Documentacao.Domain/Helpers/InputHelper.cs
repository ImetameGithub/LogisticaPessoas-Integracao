using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Helpers
{
    public static class InputHelper
    {
        public static void SelectValue(this IWebElement selectElement, string value)
        {
            try
            {
                SelectElement select = new SelectElement(selectElement);
                select.SelectByText(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
