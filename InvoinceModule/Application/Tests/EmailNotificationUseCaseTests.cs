using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InvoinceModule.Application.Tests
{
    public class EmailNotificationUseCaseTests
    {
        private readonly Mock<IInvoiceRepositoryOutPort> _repoMock = new();
        private readonly Mock<IEmailSenderOutPort> _emailSenderMock = new();
        private readonly Mock<ILogger<EmailNotificationUseCase>> _loggerMock = new();

        private readonly EmailNotificationUseCase _useCase;

        public EmailNotificationUseCaseTests()
        {
            _useCase = new EmailNotificationUseCase(
                _repoMock.Object,
                _emailSenderMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task SendPendingInvoiceNotificationsAsync_ShouldSendEmailAndUpdateInvoice()
        {
            // Arrange
            var invoice = new InvoiceHeader
            {
                InvoiceNumber = "INV-001",
                CustomerEmail = "test@example.com",
                Details = new List<InvoiceDetail> { new InvoiceDetail() },
                IsMailSent = false
            };

            _repoMock.Setup(r => r.GetProcessedButNotNotifiedInvoicesAsync())
                     .ReturnsAsync(new List<InvoiceHeader> { invoice });

            // Act
            await _useCase.SendPendingInvoiceNotificationsAsync();

            // Assert
            _emailSenderMock.Verify(s => s.SendEmailAsync(
                invoice.CustomerEmail,
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);

            _repoMock.Verify(r => r.UpdateAsync(invoice), Times.Once);
            Assert.True(invoice.IsMailSent); 
        }

        [Fact]
        public async Task SendPendingInvoiceNotificationsAsync_ShouldDoNothing_WhenNoInvoices()
        {
            // Arrange
            _repoMock.Setup(r => r.GetProcessedButNotNotifiedInvoicesAsync())
                     .ReturnsAsync(new List<InvoiceHeader>());

            // Act
            await _useCase.SendPendingInvoiceNotificationsAsync();

            // Assert
            _emailSenderMock.Verify(s => s.SendEmailAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<InvoiceHeader>()), Times.Never);
        }
    }

}
