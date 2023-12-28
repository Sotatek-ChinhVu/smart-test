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
                { TenantEnum.Type, "RDSクラスターモード" },
                { TenantEnum.Size, "利用量" },
                { TenantEnum.StorageFull, "利用量（％）" },
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
                { AuditLogEnum.LogType, "状態" },
                { AuditLogEnum.UserId , "ログ日" },
                { AuditLogEnum.LoginKey , "ユーザー ID" },
                { AuditLogEnum.LogDate , "ログインキー" },
                { AuditLogEnum.EventCd , "イベントコード" },
                { AuditLogEnum.PtId , "PtID" },
                { AuditLogEnum.SinDay , "SinDay" },
                { AuditLogEnum.RequestInfo , "リクエスト情報" },
                { AuditLogEnum.Desciption , "説明" },
                { AuditLogEnum.HpId , "HpId" },
                { AuditLogEnum.RaiinNo , "RaiinNo" },
                { AuditLogEnum.ClientIP , "ClientIP" },
                { AuditLogEnum.ThreadId , "ThreadId" },
                { AuditLogEnum.DepartmentId , "DepartmentId" },
                { AuditLogEnum.Path , "Path" },
                { AuditLogEnum.LogId , "LogId" },
            };
            return result;
        }
    }
}
