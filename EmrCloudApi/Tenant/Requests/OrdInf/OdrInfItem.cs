namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class OdrInfItem
    {
        public long Id { get; set; }
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int HokenPid { get; set; }
        public int OdrKouiKbn { get; set; }
        public string RpName { get; set; } = string.Empty;
        public int InoutKbn { get; set; }
        public int SikyuKbn { get; set; }
        public int SyohoSbt { get; set; }
        public int SanteiKbn { get; set; }
        public int TosekiKbn { get; set; }
        public int DaysCnt { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public int Status { get; set; }
        public List<OdrInfDetailItem> OdrDetails { get; set; } = new();
    }
}
