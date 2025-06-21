using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Domain.Entities;

namespace InvoinceModule.Application.Ports.Out
{
    public interface IInvoiceRepositoryOutPort
    {
        Task<InvoiceHeader> AddAsync(InvoiceHeader invoice);
        Task<IEnumerable<InvoiceHeader>> GetHeadersAsync();
        Task<InvoiceHeader> GetByInvoiceNumberAsync(string invoiceNumber);
        Task SaveChangesAsync();
    }
}
