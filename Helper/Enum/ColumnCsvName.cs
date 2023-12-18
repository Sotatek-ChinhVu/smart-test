namespace Helper.Enum;

public static class ColumnCsvName
{
    public static Dictionary<TenantEnum, string> ColumnNameTenantDictionary
    {
        get
        {
            Dictionary<TenantEnum, string> result = new()
            {
                { TenantEnum.CreateDate, "Created date" },
                { TenantEnum.TenantId, "TenantID" },
                { TenantEnum.Domain, "Domain" },
                { TenantEnum.AdminId, "AdminID" },
                { TenantEnum.HospitalName, "Hospital name" },
                { TenantEnum.Type, "RDS Cluster Mode" },
                { TenantEnum.Size, "Data size" },
                { TenantEnum.StorageFull, "Storage full" },
                { TenantEnum.StatusTenant, "Status" }
            };
            return result;
        }
    }

    public static Dictionary<AuditLogEnum, string> ColumnNameAuditLogDictionary
    {
        get
        {
            Dictionary<AuditLogEnum, string> result = new()
            {
                { AuditLogEnum.LogType, "Status" },
                { AuditLogEnum.UserId , "User ID" },
                { AuditLogEnum.LoginKey , "LoginKey" },
                { AuditLogEnum.LogDate , "Log Date" },
                { AuditLogEnum.EventCd , "EventCode" },
                { AuditLogEnum.PtId , "PtID" },
                { AuditLogEnum.SinDay , "SinDay" },
                { AuditLogEnum.RequestInfo , "Request Info" },
                { AuditLogEnum.Desciption , "Desciption" },
            };
            return result;
        }
    }
}
