using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SuperAdminModels.Tenant
{
    public class TenantModel
    {
        public TenantModel()
        {
        }

        public TenantModel(int tenantId, string hospital, byte status, int adminId, string subDomain, string db, byte type, string endPointDb, string endSubDomain, int action)
        {
            TenantId = tenantId;
            Hospital = hospital;
            Status = status;
            AdminId = adminId;
            SubDomain = subDomain;
            Db = db;
            Type = type;
            EndPointDb = endPointDb;
            EndSubDomain = endSubDomain;
            Action = action;
        }

        public int TenantId { get; set; }

        public string Hospital { get; set; } = string.Empty;

        public byte Status { get; set; }

        public int AdminId { get; set; }

        public string SubDomain { get; set; } = string.Empty;

        public string Db { get; set; } = string.Empty;

        public byte Type { get; set; }

        public string EndPointDb { get; set; } = string.Empty;

        public string EndSubDomain { get; set; } = string.Empty;

        public int Action { get; set; }
    }
}
