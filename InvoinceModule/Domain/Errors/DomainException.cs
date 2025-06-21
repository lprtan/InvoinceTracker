using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Domain.Errors
{
    public class DomainException : Exception
    {
        public int StatusCode { get; }

        public DomainException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
