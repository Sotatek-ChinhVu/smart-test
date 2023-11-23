﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SuperAdminModels.Tenant
{
    public interface INotificationRepository
    {
        void ReleaseResource();
        bool CreateNotification(byte status, string messenge);
    }
}
