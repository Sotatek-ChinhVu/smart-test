using Domain.Models.Accounting;
using EmrCloudApi.Requests.ExportPDF;

namespace EmrCloudApi.Requests.Accounting;

public class UpdateAccountingFormMstRequest
{
    public List<AccountingFormMstModel> AccountingFormMstModels { get; set; } = new();
}
