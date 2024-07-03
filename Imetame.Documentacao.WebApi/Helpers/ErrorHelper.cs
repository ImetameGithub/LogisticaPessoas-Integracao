using System.Text;

namespace Imetame.Documentacao.WebAPI.Helpers
{
    public class ErrorHelper
    {
        public static StringBuilder GetErroModelState(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.ValueEnumerable features)
        {
            try
            {
                StringBuilder erro = new StringBuilder();

                var listaerror = new List<string>();

                foreach (var modelStateVal in features)
                {
                    listaerror.AddRange(modelStateVal.Errors.Select(error => error.ErrorMessage));
                }

                foreach (var _zica in listaerror)
                {
                    erro.AppendLine(_zica.ToString());
                }

                return erro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetException(Exception ex)
        {
            try
            {
                return (ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
