using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Application.Dtos
{
    public class InvoiceHeaderDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
    }
}
