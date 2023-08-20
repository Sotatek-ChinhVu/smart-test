namespace EmrCloudApi.Requests.ExportPDF
{
    public class PeriodReceiptCsvRequest : ReportRequestBase
    {
        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int MiseisanKbn { get; set; }

        public int SaiKbn { get; set; }

        public int MisyuKbn { get; set; }

        public int SeikyuKbn { get; set; }

        public List<(long ptId, int hokenId)> PtConditions { get; set; } = new();
            
        public List<(int grpId, string grpCd)> GrpConditions { get; set; } = new();

        public int Sort { get; set; }

        public int HokenKbn { get; set; }
    }
}
