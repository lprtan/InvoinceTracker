using InvoinceModule.Domain.Entities;
using InvoinceModule.Domain.Errors;
using InvoinceModule.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InvoinceModule.Domain.Tests
{
    public class InvoiceHeaderValidatorTests
    {
        [Fact]
        public void Validate_WhenInvoiceDateIsInFuture()
        {
            // Arrange
            var invoice = new InvoiceHeader
            {
                InvoiceDate = DateTime.UtcNow.AddDays(1),
                Details = new List<InvoiceDetail> { new InvoiceDetail() }
            };

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => InvoinceHeaderVO.Validate(invoice));
            Assert.Equal("InvoiceDate gelecekte olamaz.", ex.Message);
        }

        [Fact]
        public void Validate_WhenDetailsAreEmpty()
        {
            // Arrange
            var invoice = new InvoiceHeader
            {
                InvoiceDate = DateTime.UtcNow,
                Details = new List<InvoiceDetail>()
            };

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => InvoinceHeaderVO.Validate(invoice));
            Assert.Equal("En az bir ürün detayı olmalı.", ex.Message);
        }

        [Fact]
        public void Validate_WhenInvoiceIsValid()
        {
            // Arrange
            var invoice = new InvoiceHeader
            {
                InvoiceDate = DateTime.UtcNow,
                Details = new List<InvoiceDetail> { new InvoiceDetail() }
            };

            // Act & Assert
            var ex = Record.Exception(() => InvoinceHeaderVO.Validate(invoice));
            Assert.Null(ex);
        }
    }
}
