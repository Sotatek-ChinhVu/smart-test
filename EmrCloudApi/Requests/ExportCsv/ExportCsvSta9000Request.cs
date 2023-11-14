using Reporting.CommonMasters.Enums;
using Reporting.Statistics.Sta9000.Models;

namespace EmrCloudApi.Requests.ExportCsv
{
    public class ExportCsvSta9000Request
    {
        public ExportCsvSta9000Request(List<string> outputColumns, bool isPutColName, string outputFileName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf, CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf, CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3)
        {
            OutputColumns = outputColumns;
            IsPutColName = isPutColName;
            OutputFileName = outputFileName;
            PtConf = ptConf;
            HokenConf = hokenConf;
            ByomeiConf = byomeiConf;
            RaiinConf = raiinConf;
            SinConf = sinConf;
            KarteConf = karteConf;
            KensaConf = kensaConf;
            PtIds = ptIds;
        }

        public List<string> OutputColumns { get; set; }

        public bool IsPutColName { get; set; }

        public string OutputFileName { get; set; }

        public CoSta9000PtConf? PtConf { get; set; }

        public CoSta9000HokenConf? HokenConf { get; set; }

        public CoSta9000ByomeiConf? ByomeiConf { get; set; }

        public CoSta9000RaiinConf? RaiinConf { get; set; }

        public CoSta9000SinConf? SinConf { get; set; }

        public CoSta9000KarteConf? KarteConf { get; set; }

        public CoSta9000KensaConf? KensaConf { get; set; }

        public List<long> PtIds { get; set; }

        public int SortOrder { get; set; }

        public int SortOrder2 { get; set; }

        public int SortOrder3 { get; set; }
    }
}
