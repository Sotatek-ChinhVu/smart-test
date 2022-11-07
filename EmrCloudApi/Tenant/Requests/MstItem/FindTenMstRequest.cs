namespace EmrCloudApi.Tenant.Requests.MstItem
{
    public class FindTenMstRequest
    {
        public int HpId { get; set; }
        public int SinDate { get; set; }
        public string ItemCd { get; set; } = string.Empty;
    }
}
