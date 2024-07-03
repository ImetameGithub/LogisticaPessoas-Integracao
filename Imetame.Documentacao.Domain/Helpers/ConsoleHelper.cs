using Imetame.Documentacao.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLRConsole = System.Console;

namespace Imetame.Documentacao.Domain.Helpers
{
    public class ConsoleHelper
    {
        private readonly ILogProcessamentoRepository _logProcessamentoRepository;

        public ConsoleHelper(ILogProcessamentoRepository logProcessamentoRepository)
        {
            _logProcessamentoRepository = logProcessamentoRepository;
        }

        public void Log(string value, Guid idProcessamento)
        {
            var log = new Entities.LogProcessamento()
            {
                IdProcessamento = idProcessamento,
            };
            log.Evento = $"[{log.DataEvento.ToString("dd/MM/yyyy HH:mm:ss")}] - {value}";
            CLRConsole.WriteLine(log.Evento);
            _logProcessamentoRepository.Add(log);
            _logProcessamentoRepository.SaveChanges();
        }
        public void WriteLine(string value)
        {
            CLRConsole.WriteLine(value);
        }

    }
}
