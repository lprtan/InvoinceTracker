using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoinceModule.Application.Dtos;
using InvoinceModule.Application.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace InvoinceModule.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceHeaderInputPort _invoiceHeaderInputPort;
        public InvoiceController(IInvoiceHeaderInputPort invoiceHeaderInputPort)
        {
            _invoiceHeaderInputPort = invoiceHeaderInputPort;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoiceAsync([FromBody] InvoiceDto invoiceDto)
        {
            var createdInvoice = await _invoiceHeaderInputPort.CreateInvoiceAsync(invoiceDto);
            return Ok(createdInvoice);
        }

        [HttpGet("{invoiceNumber}")]
        public async Task<IActionResult> GetInvoiceByIdAsync(string invoiceNumber)
        {
            var invoice = await _invoiceHeaderInputPort.GetInvoiceByIdAsync(invoiceNumber);
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceHeaderInputPort.GetInvoiceHeadersAsync();
            return Ok(invoices);
        }
    }
}
