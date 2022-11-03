namespace EmrCloudApi.Tenant.Requests.InsuranceMst
{
    public class SearchHokensyaMstRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public string? Keyword { get; set; }
    }
}
