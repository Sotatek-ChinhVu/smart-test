namespace EmrCloudApi.Requests.ExportPDF;

public class StatisticExportRequest
{
    public int HpId { get; set; }

    public int MenuId { get; set; }

    public int MonthFrom { get; set; }

    public int MonthTo { get; set; }

    public int DateFrom { get; set; }

    public int DateTo { get; set; }

    public int TimeFrom { get; set; }

    public int TimeTo { get; set; }
}
