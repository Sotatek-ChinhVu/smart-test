namespace Helper.Enum;

public static class ColumnCsvName
{
    public static Dictionary<TenantEnum, string> ColumnNameTenantDictionary
    {
        get
        {
            Dictionary<TenantEnum, string> result = new()
            {
                { TenantEnum.CreateDate, "作成日" },
                { TenantEnum.TenantId, "医療機関ID" },
                { TenantEnum.Domain, "ドメイン" },
                { TenantEnum.AdminId, "管理者ID" },
                { TenantEnum.HospitalName, "医療機関名" },
                { TenantEnum.StatusTenant, "ステータス" }
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
                { AuditLogEnum.LogDate , "LogDate" },
                { AuditLogEnum.UserId , "UserId" },
                { AuditLogEnum.LoginKey , "LoginKey" },
                { AuditLogEnum.EventCd , "EventCd" },
                { AuditLogEnum.PtId , "PtID" },
                { AuditLogEnum.SinDay , "SinDay" },
                { AuditLogEnum.RequestInfo , "RequestInfo" },
                { AuditLogEnum.Desciption , "Desciption" },
                { AuditLogEnum.HpId , "HpId" },
                { AuditLogEnum.RaiinNo , "RaiinNo" },
                { AuditLogEnum.ClientIP , "ClientIP" },
                { AuditLogEnum.ThreadId , "ThreadId" },
                { AuditLogEnum.DepartmentId , "DepartmentId" },
                { AuditLogEnum.Path , "Path" },
                { AuditLogEnum.LogId , "LogID" },
            };
            return result;
        }
    }
}
