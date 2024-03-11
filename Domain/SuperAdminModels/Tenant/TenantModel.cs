using Helper.Constants;

namespace Domain.SuperAdminModels.Tenant
{
    public class TenantModel
    {
        public TenantModel()
        {
            Hospital = string.Empty;
            Password = string.Empty;
            SubDomain = string.Empty;
            Db = string.Empty;
            EndPointDb = string.Empty;
            EndSubDomain = string.Empty;
            RdsIdentifier = string.Empty;
        }

        public TenantModel(int tenantId, string hospital, byte status, int adminId, string password, string subDomain, string db, string endPointDb, string endSubDomain, int action, int scheduleDate, int scheduleTime, DateTime createDate, string rdsIdentifier, bool isRestoreS3)
        {
            TenantId = tenantId;
            Hospital = hospital;
            Status = status;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            Db = db;
            EndPointDb = endPointDb;
            EndSubDomain = endSubDomain;
            Action = action;
            ScheduleDate = scheduleDate;
            ScheduleTime = scheduleTime;
            CreateDate = createDate;
            RdsIdentifier = rdsIdentifier;
            IsRestoreS3 = isRestoreS3;

        }

        public TenantModel(int tenantId, string hospital, byte status, int adminId, string password, string subDomain, string db, string endPointDb, string endSubDomain, int action, string rdsIdentifier)
        {
            TenantId = tenantId;
            Hospital = hospital;
            Status = status;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            Db = db;
            EndPointDb = endPointDb;
            EndSubDomain = endSubDomain;
            Action = action;
            RdsIdentifier = rdsIdentifier;
        }

        public TenantModel ChangeRdsIdentifier(string rdsIdentifier)
        {
            RdsIdentifier = rdsIdentifier;
            return this;
        }

        public int TenantId { get; private set; }

        public string Hospital { get; private set; }

        public byte Status { get; private set; }

        public int AdminId { get; private set; }

        public string Password { get; private set; }

        public string SubDomain { get; private set; }

        public string Db { get; private set; }

        public string EndPointDb { get; private set; }

        public string EndSubDomain { get; private set; }

        public int Action { get; private set; }

        public int ScheduleDate { get; private set; }

        public int ScheduleTime { get; private set; }

        public DateTime CreateDate { get; private set; }

        public string RdsIdentifier { get; private set; }

        public bool IsRestoreS3 { get; private set; }

        /// <summary>
        /// Return StatusTenant to FE
        /// </summary>
        public byte StatusTenant
        {
            get
            {
                return (byte)(StatusTenantDisplayConst.StatusTenantDisplayDictionnary.ContainsKey(Status) ? StatusTenantDisplayConst.StatusTenantDisplayDictionnary[Status] : 0);
            }
        }
    }
}
