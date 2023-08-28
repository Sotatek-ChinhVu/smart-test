using Reporting.CommonMasters.Enums;

namespace EmrCloudApi.Requests.ExportCsv
{
    public class ExportCsvStaticsRequest
    {
        public int MenuId { get; set; }

        public int? MonthFrom { get; set; } = 0;

        public int? MonthTo { get; set; } = 0;

        public int? DateFrom { get; set; } = 0;

        public int? DateTo { get; set; } = 0;

        public int TimeFrom { get; set; }

        public int TimeTo { get; set; }

        public bool? IsPutTotalRow { get; set; } = false;

        public int? TenkiDateFrom { get; set; } = -1;

        public int? TenkiDateTo { get; set; } = -1;

        public int? EnableRangeFrom { get; set; } = -1;

        public int? EnableRangeTo { get; set; } = -1;

        public long? PtNumFrom { get; set; } = 0;

        public long? PtNumTo { get; set; } = 0;

        public string MenuName { get; set; } = string.Empty;

        public bool? IsPutColName { get; set; } = false;

        public CoFileType? CoFileType { get; set; } = null;
    }
}
