using Reporting.CommonMasters.Enums;

namespace EmrCloudApi.Requests.ExportPDF;

public class AccountingCoReportRequest
{
    public int HpId { get; set; }

    public ConfirmationMode Mode { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public List<CoAccountDueListRequestModel> AccountDueListModels { get; set; } = new();

    public List<CoAccountDueListRequestModel> MultiAccountDueListModels { get; set; } = new();

    public CoAccountDueListRequestModel SelectedAccountDueListModel { get; set; } = new();

    public bool IsRyosyoDetail { get; set; }

    public int PtRyosyoDetail { get; set; }

    public bool IsPrintMonth { get; set; }
}

public class CoAccountDueListRequestModel
{
    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public long OyaRaiinNo { get; set; }
}
