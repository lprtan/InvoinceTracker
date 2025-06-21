using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Domain.Entities;
using InvoinceModule.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InvoinceModule.Infrastructure.Adapters.Invoince
{
    public class InvoiceRepository : IInvoiceRepositoryOutPort
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceHeader> AddAsync(InvoiceHeader invoice)
        {
            var entry = await _context.InvoiceHeaders.AddAsync(invoice);
            return entry.Entity;
        }

        public async Task<IEnumerable<InvoiceHeader>> GetHeadersAsync()
        {
            return await _context.InvoiceHeaders
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InvoiceHeader> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.InvoiceHeaders
                .Include(i => i.Details)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvoiceHeader>> GetProcessedButNotNotifiedInvoicesAsync()
        {
            return await _context.InvoiceHeaders
                .Include(i => i.Details)
                .Where(i => i.IsProcessed && !i.IsMailSent)
                .ToListAsync();
        }

        public async Task UpdateAsync(InvoiceHeader invoice)
        {
            _context.InvoiceHeaders.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}
