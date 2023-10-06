using Reporting.Accounting.Model;

namespace EmrCloudApi.Requests.ExportPDF;

public class PeriodReceiptListRequest : ReportRequestBase
{
    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public List<PtInfInputItem> SourcePt { get; set; } = new();

    public List<GrpInputItem> GrpConditions { get; set; } = new();

    public int PrintSort { get; set; }

    public bool IsPrintList { get; set; }

    public bool PrintByMonth { get; set; }

    public bool PrintByGroup { get; set; }

    public int MiseisanKbn { get; set; }

    public int SaiKbn { get; set; }

    public int MisyuKbn { get; set; }

    public int SeikyuKbn { get; set; }

    public int HokenKbn { get; set; }

    public int HakkoDay { get; set; }

    public string Memo { get; set; } = string.Empty;

    public string FormFileName { get; set; } = string.Empty;

    public bool NyukinBase { get; set; }
}
