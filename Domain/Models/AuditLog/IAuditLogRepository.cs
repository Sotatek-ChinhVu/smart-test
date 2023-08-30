using Domain.Common;
using Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.AuditLog
{
    public interface IAuditLogRepository : IRepositoryBase
    {
        bool SaveAuditLog(int hpId, int userId, AuditTrailLogModel auditTrailLogModel);
    }
}
