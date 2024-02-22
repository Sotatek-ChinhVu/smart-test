using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public class SearchTenantModel
{
    public SearchTenantModel(string keyWord, DateTime? fromDate, DateTime? toDate, int statusTenant)
    {
        KeyWord = keyWord;
        FromDate = fromDate;
        ToDate = toDate;
        StatusTenant = statusTenant;
    }

    public string KeyWord { get; private set; }

    public DateTime? FromDate { get; private set; }

    public DateTime? ToDate { get; private set; }

    public int StatusTenant { get; private set; }

    public bool IsEmptyModel
    {
        get
        {
            return StatusTenant == -1
                   && FromDate == null
                   && ToDate == null
                   && string.IsNullOrEmpty(KeyWord);
        }
    }
}
