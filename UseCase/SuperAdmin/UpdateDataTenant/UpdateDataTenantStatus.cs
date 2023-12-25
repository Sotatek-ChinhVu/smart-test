﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SuperAdmin.UpdateDataTenant
{
    public enum UpdateDataTenantStatus : byte
    {
        Successed = 1,
        InvalidTenantId = 2,
        Failed = 3,
        TenantDoesNotExist = 4,
        TenantNotAvailable = 5,
        UploadFileIncorrectFormat7z = 6,
    }
}