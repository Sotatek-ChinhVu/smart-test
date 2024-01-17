using Reporting.CommonMasters.Enums;

namespace EmrCloudApi.Requests.ExportPDF;

public class StatisticExportRequest
{
    public int MenuId { get; set; }

    public int MonthFrom { get; set; }

    public int MonthTo { get; set; }

    public int DateFrom { get; set; }

    public int DateTo { get; set; }

    public int TimeFrom { get; set; }

    public int TimeTo { get; set; }

    public CoFileType? CoFileType { get; set; } = null;

    public bool? IsPutTotalRow { get; set; } = false;

    public int? TenkiDateFrom { get; set; } = -1;

    public int? TenkiDateTo { get; set; } = -1;

    public int? EnableRangeFrom { get; set; } = -1;

    public int? EnableRangeTo { get; set; } = -1;

    public long? PtNumFrom { get; set; } = 0;

    public long? PtNumTo { get; set; } = 0;

    public string FormName { get; set; } = string.Empty;
}
