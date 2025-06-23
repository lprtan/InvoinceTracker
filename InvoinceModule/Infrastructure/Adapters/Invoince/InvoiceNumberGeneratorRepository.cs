using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoinceModule.Infrastructure.Adapters.Invoince
{
    public class InvoiceNumberGeneratorRepository : IInvoiceNumberGeneratorOutPort
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InvoiceNumberGeneratorRepository> _logger;

        public InvoiceNumberGeneratorRepository(AppDbContext context, ILogger<InvoiceNumberGeneratorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetNextSequenceAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var sequenceEntry = await _context.InvoiceNumberSequences
                    .FromSqlRaw("SELECT * FROM InvoiceNumberSequences WITH (UPDLOCK, ROWLOCK) WHERE Id = 1")
                    .SingleAsync();

                int nextNumber = sequenceEntry.LastNumber + 1;
                sequenceEntry.LastNumber = nextNumber;
                _context.InvoiceNumberSequences.Update(sequenceEntry);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Sequence number generated successfully: {NextNumber}", nextNumber);

                return nextNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating next sequence number.");
                throw;
            }
        }
    }
}
