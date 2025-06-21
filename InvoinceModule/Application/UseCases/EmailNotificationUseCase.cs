using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Infrastructure.Adapters.Invoince;

namespace InvoinceModule.Application.UseCases
{
    public class EmailNotificationUseCase
    {
        private readonly IInvoiceRepositoryOutPort _invoiceRepository;
        private readonly IEmailSender _emailSender;

        public EmailNotificationUseCase(IInvoiceRepositoryOutPort invoiceRepository, IEmailSender emailSender)
        {
            this._invoiceRepository = invoiceRepository;
            this._emailSender = emailSender;
        }

        public async Task SendPendingInvoiceNotificationsAsync()
        {
            var invoices = await _invoiceRepository.GetProcessedButNotNotifiedInvoicesAsync();

            foreach (var invoice in invoices)
            {
                var subject = $"Fatura {invoice.InvoiceNumber} İşlem Bildirimi";
                var body = $"{invoice.Details.Count} kalem ürün içeren {invoice.InvoiceNumber} nolu faturanız başarıyla işlenmiştir.";

                await _emailSender.SendEmailAsync(invoice.CustomerEmail, subject, body);

                invoice.IsMailSent = true;
                await _invoiceRepository.UpdateAsync(invoice);
            }
        }
    }
}
