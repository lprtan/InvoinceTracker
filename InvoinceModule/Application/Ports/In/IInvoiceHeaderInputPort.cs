using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Dtos;

namespace InvoinceModule.Application.Ports.In
{
    public interface IInvoiceHeaderInputPort
    {
        Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoice);

        Task<IEnumerable<InvoiceHeaderDto>> GetInvoiceHeadersAsync();

        Task<InvoiceDto> GetInvoiceByIdAsync(string invoiceNumber);
    }
}
