namespace Domain.SuperAdminModels.Tenant
{
    public class TenantModel
    {
        public TenantModel()
        {
        }

        public TenantModel(int tenantId, string hospital, byte status, int adminId, string password, string subDomain, string db, int size, byte type, string endPointDb, string endSubDomain, int action, string rdsIdentifier)
        {
            TenantId = tenantId;
            Hospital = hospital;
            Status = status;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            Db = db;
            Size = size;
            Type = type;
            EndPointDb = endPointDb;
            EndSubDomain = endSubDomain;
            Action = action;
            RdsIdentifier = rdsIdentifier;
        }
        public TenantModel(string hospital, byte status, int adminId, string password, string subDomain, string db, int size, int sizeType, byte type, string endPointDb, string endSubDomain, int action, string rdsIdentifier)
        {
            Hospital = hospital;
            Status = status;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            Db = db;
            Size = size;
            SizeType = sizeType;
            Type = type;
            EndPointDb = endPointDb;
            EndSubDomain = endSubDomain;
            Action = action;
            RdsIdentifier = rdsIdentifier;
        }

        public int TenantId { get; set; }

        public string Hospital { get; set; } = string.Empty;

        public byte Status { get; set; }

        public int AdminId { get; set; }
        public string Password { get; set; } = string.Empty;

        public string SubDomain { get; set; } = string.Empty;

        public string Db { get; set; } = string.Empty;

        public int Size { get; set; }

        /// <summary>
        /// 1:MB ; 2:GB
        /// </summary>
        public int SizeType { get; set; }

        /// <summary>
        /// 1:sharing; 2:dedicate(premium)
        /// </summary>
        public byte Type { get; set; }

        public string EndPointDb { get; set; } = string.Empty;

        public string EndSubDomain { get; set; } = string.Empty;

        public int Action { get; set; }

        public string RdsIdentifier { get; set; } = string.Empty;
    }
}
