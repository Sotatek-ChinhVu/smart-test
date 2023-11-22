using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public class SearchTenantModel
{
    public SearchTenantModel(string keyWord, DateTime? fromDate, DateTime? toDate, int type, int status, StorageFullEnum storageFull)
    {
        KeyWord = keyWord;
        FromDate = fromDate;
        ToDate = toDate;
        Type = type;
        Status = status;
        StorageFull = storageFull;
    }

    public string KeyWord { get; private set; }

    public DateTime? FromDate { get; private set; }

    public DateTime? ToDate { get; private set; }

    public int Type { get; private set; }

    public int Status { get; private set; }

    public StorageFullEnum StorageFull { get; private set; }

    public bool IsEmptyModel
    {
        get
        {
            return StorageFull == StorageFullEnum.Empty
                   && Status == -1
                   && Type == -1
                   && FromDate == null
                   && ToDate == null
                   && string.IsNullOrEmpty(KeyWord);
        }
    }
}
