using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InvoinceModule.Application.Helpers
{
    public static class InvoiceNumberGenerator
    {
        public static string Generate(int number)
        {
            return $"SVS{DateTime.Now.Year}{number}";
        }
    }
}
