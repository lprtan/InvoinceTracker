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
        _logger.LogInformation("Starting to send emails for pending invoices.");

        try
        {
            var invoices = await _invoiceRepository.GetProcessedButNotNotifiedInvoicesAsync();

            if (!invoices.Any())
            {
                _logger.LogInformation("No pending invoices found to send emails for.");
                return;
            }

            foreach (var invoice in invoices)
            {
                var subject = $"Invoice {invoice.InvoiceNumber} Processing Notification";
                var body = $"{invoice.Details.Count} items in your invoice {invoice.InvoiceNumber} have been processed successfully.";

                _logger.LogInformation("Sending email for invoice '{InvoiceNumber}' to {CustomerEmail}.", invoice.InvoiceNumber, invoice.CustomerEmail);

                await _emailSender.SendEmailAsync(invoice.CustomerEmail, subject, body);

                invoice.IsMailSent = true;
                await _invoiceRepository.UpdateAsync(invoice);

                _logger.LogInformation("Email sent successfully for invoice '{InvoiceNumber}'.", invoice.InvoiceNumber);
            }

            _logger.LogInformation("Finished sending emails for all pending invoices.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending invoice emails.");
            throw;
        }
    }
}
