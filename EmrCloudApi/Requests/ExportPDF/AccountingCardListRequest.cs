using Reporting.AccountingCardList.Model;

namespace EmrCloudApi.Requests.ExportPDF;

public class AccountingCardListRequest
{
    public List<TargetItem> Targets { get; set; } = new();

    public bool IncludeOutDrug { get; set; }

    public string KaName { get; set; } = string.Empty;

    public string TantoName { get; set; } = string.Empty;

    public string UketukeSbt { get; set; } = string.Empty;

    public string Hoken { get; set; } = string.Empty;
}
