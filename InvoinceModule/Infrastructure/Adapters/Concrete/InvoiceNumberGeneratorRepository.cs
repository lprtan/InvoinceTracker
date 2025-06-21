using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace InvoinceModule.Infrastructure.Adapters.Concrete
{
    public class InvoiceNumberGeneratorRepository : IInvoiceNumberGeneratorOutPort
    {
        private readonly AppDbContext _context;

        public InvoiceNumberGeneratorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetNextSequenceAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var sequenceEntry = await _context.InvoiceNumberSequences
                .FromSqlRaw("SELECT * FROM InvoiceNumberSequences WITH (UPDLOCK, ROWLOCK) WHERE Id = 1")
                .SingleAsync();

            int nextNumber = sequenceEntry.LastNumber + 1;
            sequenceEntry.LastNumber = nextNumber;
            _context.InvoiceNumberSequences.Update(sequenceEntry);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return nextNumber;
        }
    }
}
