using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Application.Ports.Out
{
    public interface IInvoiceNumberGeneratorOutPort
    {
        Task<int> GetNextSequenceAsync();
    }
}
