namespace EmrCloudApi.Requests.MstItem
{
    public class GetSetDataTenMstRequest
    {
        public int SinDate { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string JiCd { get; set; } = string.Empty;

        public string IpnNameCd { get; set; } = string.Empty;

        public string SanteiItemCd { get; set; } = string.Empty;

        public string AgekasanCd1Note { get; set; } = string.Empty;

        public string AgekasanCd2Note { get; set; } = string.Empty;

        public string AgekasanCd3Note { get; set; } = string.Empty;

        public string AgekasanCd4Note { get; set; } = string.Empty;
    }
}
