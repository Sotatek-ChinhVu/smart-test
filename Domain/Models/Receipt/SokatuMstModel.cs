namespace Domain.Models.Receipt
{
    public class SokatuMstModel
    {
        public SokatuMstModel(int prefNo, int startYm, int endYm, int reportId, int reportEdaNo, int sortNo, string reportName, int printType, int printNoType, int dataAll, int dataDisk, int dataPaper, int dataKbn, string diskKind, int diskCnt, int isSort)
        {
            PrefNo = prefNo;
            StartYm = startYm;
            EndYm = endYm;
            ReportId = reportId;
            ReportEdaNo = reportEdaNo;
            SortNo = sortNo;
            ReportName = reportName;
            PrintType = printType;
            PrintNoType = printNoType;
            DataAll = dataAll;
            DataDisk = dataDisk;
            DataPaper = dataPaper;
            DataKbn = dataKbn;
            DiskKind = diskKind;
            DiskCnt = diskCnt;
            IsSort = isSort;
        }

        public int PrefNo { get; private set; }

        public int StartYm { get; private set; }

        public int EndYm { get; private set; }

        public int ReportId { get; private set; }

        public int ReportEdaNo { get; private set; }

        public int SortNo { get; private set; }

        public string ReportName { get; private set; }

        public int PrintType { get; private set; }

        public int PrintNoType { get; private set; }

        public int DataAll { get; private set; }

        public int DataDisk { get; private set; }

        public int DataPaper { get; private set; }

        public int DataKbn { get; private set; }

        public string DiskKind { get; private set; }

        public int DiskCnt { get; private set; }

        public int IsSort { get; private set; }
    }
}
