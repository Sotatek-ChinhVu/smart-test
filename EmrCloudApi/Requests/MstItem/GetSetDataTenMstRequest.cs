namespace EmrCloudApi.Requests.MstItem
{
    /// <summary>
    /// .NET 6 [FromQuery] return null values when binding to object
    /// </summary>
    public class GetSetDataTenMstRequest
    {
        public int SinDate { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string JiCd { get; set; } = string.Empty;

        public string? IpnNameCd { get; set; }

        public string? SanteiItemCd { get; set; }

        public string? AgekasanCd1Note { get; set; }

        public string? AgekasanCd2Note { get; set; }

        public string? AgekasanCd3Note { get; set; }

        public string? AgekasanCd4Note { get; set; }
    }
}
