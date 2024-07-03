using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public List<string> Messages { get; set; }
        public DomainException()
        { Messages = new List<string>(); }

        public DomainException(string message)
            : base(message)
        {
            Messages = new List<string>();
            Messages.Add(message);
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
            Messages = new List<string>();
            Messages.Add(message);
        }
        public DomainException(List<string> messages)
        {
            Messages = new List<string>();
            if (messages != null)
                Messages.AddRange(messages);
        }

    }
}
