using Entity.Tenant;

namespace EmrCalculateApi.Requests
{
    public class OrderInfo
    {
        public List<OrderDetailInfo> DetailInfoList { get; set; } = new List<OrderDetailInfo>();

        public long RpNo { get; set; }

        public long RpEdaNo { get; set; }

        public int HokenPid { get; set; }

        public int OdrKouiKbn { get; set; }

        public int GroupOdrKouiKbn { get; set; }

        public int InoutKbn { get; set; }

        public int SikyuKbn { get; set; }

        public int SyohoSbt { get; set; }

        public int SanteiKbn { get; set; }

        public int TosekiKbn { get; set; }

        public int DaysCnt { get; set; }

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }
    }
}
