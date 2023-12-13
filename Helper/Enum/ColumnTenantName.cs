namespace Helper.Enum;

public static class ColumnTenantName
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
}
