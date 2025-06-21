using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Domain.Entities
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int InvoiceHeaderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public InvoiceHeader InvoiceHeader { get; set; }
    }
}
