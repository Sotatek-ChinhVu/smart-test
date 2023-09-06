using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SaveAuditLog
{
    public enum SaveAuditTrailLogStatus : byte
    {
        Successed = 1,
        Failed = 2,
    }
}
