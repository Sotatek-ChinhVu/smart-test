namespace EmrCloudApi.Requests.MstItem
{
    public class SearchPostCodeRequest
    {
        public string PostCode1 { get; set; } = string.Empty;
        public string PostCode2 { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int PageIndex { get; set; } = -1;
        public int PageSize { get; set; } = -1;
    }
}
