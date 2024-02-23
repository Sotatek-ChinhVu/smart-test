using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public class SearchTenantModel
{
    public SearchTenantModel(string keyWord, DateTime? fromDate, DateTime? toDate, int type, int statusTenant, List<StorageFullEnum> storageFull)
    {
        KeyWord = keyWord;
        FromDate = fromDate;
        ToDate = toDate;
        Type = type;
        StatusTenant = statusTenant;
        StorageFull = storageFull;
    }

    public string KeyWord { get; private set; }

    public DateTime? FromDate { get; private set; }

    public DateTime? ToDate { get; private set; }

    public int Type { get; private set; }

    public int StatusTenant { get; private set; }

    public List<StorageFullEnum> StorageFull { get; private set; }

    public bool IsEmptyModel
    {
        get
        {
            return !StorageFull.Any()
                   && StatusTenant == -1
                   && Type == -1
                   && FromDate == null
                   && ToDate == null
                   && string.IsNullOrEmpty(KeyWord);
        }
    }
}
