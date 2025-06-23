using InvoinceModule.Application.Ports.Out;
using Microsoft.Extensions.Logging;

public class EmailNotificationUseCase
{
    private readonly IInvoiceRepositoryOutPort _invoiceRepository;
    private readonly IEmailSenderOutPort _emailSender;
    private readonly ILogger<EmailNotificationUseCase> _logger;

    public EmailNotificationUseCase(
        IInvoiceRepositoryOutPort invoiceRepository,
        IEmailSenderOutPort emailSender,
        ILogger<EmailNotificationUseCase> logger)
    {
        _invoiceRepository = invoiceRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task SendPendingInvoiceNotificationsAsync()
    {
        _logger.LogInformation("Gönderilmemiş faturalar için mail gönderme işlemi başladı.");

        try
        {
            var invoices = await _invoiceRepository.GetProcessedButNotNotifiedInvoicesAsync();

            if (!invoices.Any())
            {
                _logger.LogInformation("Gönderilecek yeni fatura bulunamadı.");
                return;
            }

            foreach (var invoice in invoices)
            {
                var subject = $"Fatura {invoice.InvoiceNumber} İşlem Bildirimi";
                var body = $"{invoice.Details.Count} kalem ürün içeren {invoice.InvoiceNumber} nolu faturanız başarıyla işlenmiştir.";

                _logger.LogInformation("'{InvoiceNumber}' faturası için mail gönderiliyor: {CustomerEmail}", invoice.InvoiceNumber, invoice.CustomerEmail);

                await _emailSender.SendEmailAsync(invoice.CustomerEmail, subject, body);

                invoice.IsMailSent = true;
                await _invoiceRepository.UpdateAsync(invoice);

                _logger.LogInformation("'{InvoiceNumber}' faturası için mail gönderildi.", invoice.InvoiceNumber);
            }

            _logger.LogInformation("Tüm bekleyen faturalar için mail gönderme işlemi tamamlandı.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mail gönderimi sırasında hata oluştu.");
            throw;
        }
    }
}
