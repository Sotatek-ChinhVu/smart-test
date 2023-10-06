namespace EmrCloudApi.Requests.ExportCsv
{
    public class ExportCsvRequest
    {
        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public List<PtInf> PtConditions { get; set; } = new();

        public List<GroupInf> GrpConditions { get; set; } = new();

        public int Sort { get; set; }

        public int MiseisanKbn { get; set; }

        public int SaiKbn { get; set; }

        public int MisyuKbn { get; set; }

        public int SeikyuKbn { get; set; }

        public int HokenKbn { get; set; }
    }

    public class PtInf
    {
        public long PtId { get; set; }
        public int HokenId { get; set; }
    }

    public class GroupInf
    {
        public int GrpId { get; set; }
        public string GrpCd { get; set; } = string.Empty;
    }
}
