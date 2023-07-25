namespace Domain.Models.Receipt
{
    public class SokatuMstModel
    {
        public SokatuMstModel(int prefNo, int reportId, int reportEdaNo, string reportName)
        {
            PrefNo = prefNo;
            ReportId = reportId;
            ReportEdaNo = reportEdaNo;
            ReportName = reportName;
        }

        public int PrefNo { get; private set; }

        public int ReportId { get; private set; }

        public int ReportEdaNo { get; private set; }

        public string ReportName { get; private set; }
    }
}
