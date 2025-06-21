﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoinceModule.Infrastructure.Adapters.Email
{
    public class SmtpSettings
    {
            public string Server { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string FromAddress { get; set; }
            public string FromName { get; set; }
    }
}
