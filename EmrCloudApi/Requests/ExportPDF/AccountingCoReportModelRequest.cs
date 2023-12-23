using Reporting.CommonMasters.Enums;

namespace EmrCloudApi.Requests.ExportPDF;

public class AccountingCoReportModelRequest
{

    public ConfirmationMode Mode { get; set; }

    public long PtId { get; set; }

    public List<CoAccountDueListRequestModel> MultiAccountDueListModels { get; set; } = new();

    public bool IsPrintMonth { get; set; }

    public bool Ryoshusho { get; set; }

    public bool Meisai { get; set; }
}

public class CoAccountDueListRequestModel
{
    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public long OyaRaiinNo { get; set; }
}
