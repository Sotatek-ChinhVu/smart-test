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
            UserConnect = string.Empty;
            PasswordConnect = string.Empty;
        }

        public TenantModel(int tenantId, string hospital, byte status, int adminId, string password, string subDomain, string db, double size, int sizeType, byte type, string endPointDb, string endSubDomain, int action, int scheduleDate, int scheduleTime, DateTime createDate, string rdsIdentifier, string userConnect, string passwordConnect)
        {
            TenantId = tenantId;
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
            ScheduleDate = scheduleDate;
            ScheduleTime = scheduleTime;
            CreateDate = createDate;
            RdsIdentifier = rdsIdentifier;
            UserConnect = userConnect;
            PasswordConnect = passwordConnect;
        }

        public TenantModel(string hospital, byte status, int adminId, string password, string subDomain, string db, double size, int sizeType, byte type, string endPointDb, string endSubDomain, int action, string rdsIdentifier, string userConnect, string passwordConnect)
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
            UserConnect = userConnect;
            PasswordConnect = passwordConnect;
        }

        public TenantModel ChangeStorageFull(double storageFull, double storageUsed)
        {
            StorageFull = storageFull;
            StorageUsed = storageUsed;
            return this;
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

        public double Size { get; private set; }

        public int SizeType { get; private set; }

        public byte Type { get; private set; }

        public string EndPointDb { get; private set; }

        public string EndSubDomain { get; private set; }

        public int Action { get; private set; }

        public int ScheduleDate { get; private set; }

        public int ScheduleTime { get; private set; }

        public DateTime CreateDate { get; private set; }

        public string RdsIdentifier { get; private set; }

        /// <summary>
        ///  return storage used to FE
        /// </summary>
        public double StorageUsed { get; private set; }

        public double StorageFull { get; private set; }

        public string UserConnect { get; private set; }

        public string PasswordConnect { get; private set; }

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
