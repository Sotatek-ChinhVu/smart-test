namespace EmrCloudApi.Tenant.Requests.InsuranceMst
{
    public class SearchHokensyaMstRequest
    {
        public int HpId { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int SinDate { get; set; }

        public string Keyword { get; set; }
    }
}
