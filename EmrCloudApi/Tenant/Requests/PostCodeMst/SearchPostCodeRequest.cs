namespace EmrCloudApi.Tenant.Requests.PostCodeMst
{
    public class SearchPostCodeRequest
    {
        public int HpId { get; set; }
        public string PostCode1 { get; set; } = string.Empty;
        public string PostCode2 { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
