﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Messaging.Data
{
    public class StopUpdateDataTenantStatus : CallbackMessage<bool>
    {
        public StopUpdateDataTenantStatus()
        {
        }
    }
}