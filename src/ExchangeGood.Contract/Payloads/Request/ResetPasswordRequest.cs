﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Payloads.Request
{
    public class ResetPasswordRequest
    {
        public string FeId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }


    }
}
