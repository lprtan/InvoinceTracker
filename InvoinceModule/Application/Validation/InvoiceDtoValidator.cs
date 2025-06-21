using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InvoinceModule.Application.Dtos;

namespace InvoinceModule.Application.Validation
{
    public class InvoiceDtoValidator : AbstractValidator<InvoiceDto>
    {
        public InvoiceDtoValidator()
        {
            RuleFor(x => x.InvoiceDate).LessThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.CustomerEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.CustomerEmail));
            RuleFor(x => x.SenderTitle).NotEmpty();
            RuleFor(x => x.ReceiverTitle).NotEmpty();
            RuleFor(x => x.Details).NotEmpty();
            RuleForEach(x => x.Details).SetValidator(new InvoiceDetailDtoValidator());
        }
    }
}
