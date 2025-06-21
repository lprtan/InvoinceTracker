using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Domain.Entities;
using InvoinceModule.Domain.Errors;

namespace InvoinceModule.Domain.ValueObject
{
    public static class InvoinceHeaderVO
    {
        public static void Validate(InvoiceHeader invoiceHeader)
        {

            if ( invoiceHeader.InvoiceDate > DateTime.UtcNow)
                throw new DomainException("InvoiceDate gelecekte olamaz.", 400);

            if (invoiceHeader.Details == null || !invoiceHeader.Details.Any())
                throw new DomainException("En az bir ürün detayı olmalı.", 400 );
        }
    }
}
