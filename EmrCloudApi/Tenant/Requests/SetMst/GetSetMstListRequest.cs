namespace EmrCloudApi.Tenant.Requests.SetMst
{
    public class GetSetMstListRequest
    {
        public int HpId { get; set; }
        public int SetKbn { get; set; }
        public int SetKbnEdaNo { get; set; }
        public int SinDate { get; set; }
        public string TextSearch { get; set; } = string.Empty;
    }
}