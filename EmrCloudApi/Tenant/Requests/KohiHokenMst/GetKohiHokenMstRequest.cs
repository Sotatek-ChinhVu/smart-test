namespace EmrCloudApi.Tenant.Requests.KohiHokenMst
{
    public class GetKohiHokenMstRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public string FutansyaNo { get; set; } = string.Empty;
    }
}
