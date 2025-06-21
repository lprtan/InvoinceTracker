using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InvoinceModule.Application.Dtos;


namespace InvoinceModule.Application.Validation
{
    public class InvoiceDetailDtoValidator : AbstractValidator<InvoiceDetailDto>
    {
        public InvoiceDetailDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        }
    }
}
