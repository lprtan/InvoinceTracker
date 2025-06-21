using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvoinceModule.Application.Dtos
{
    public class InvoiceDto
    {
        [JsonIgnore]
        public string? InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string SenderTitle { get; set; }
        public string ReceiverTitle { get; set; }
        public List<InvoiceDetailDto> Details { get; set; }
    }
}
