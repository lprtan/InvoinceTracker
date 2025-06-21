using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Domain.Entities
{
    public class InvoiceHeader
    {
        public int Id { get; set; }                    
        public string InvoiceNumber { get; set; }       
        public DateTime InvoiceDate { get; set; }
        public string SenderTitle { get; set; }
        public string ReceiverTitle { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsProcessed { get; set; }          
        public bool IsMailSent { get; set; }             

        public ICollection<InvoiceDetail> Details { get; set; }
    }
}
