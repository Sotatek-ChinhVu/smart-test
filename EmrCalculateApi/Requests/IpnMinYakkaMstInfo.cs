namespace EmrCalculateApi.Requests
{
    public class IpnMinYakkaMstInfo
    {
        public int HpId { get; set; }

        public string IpnNameCd { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public double Yakka { get; set; }

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateId { get; set; }

        public string CreateMachine { get; set; } = string.Empty;

        public DateTime UpdateDate { get; set; }

        public int UpdateId { get; set; }

        public string UpdateMachine { get; set; } = string.Empty;
    }
}
