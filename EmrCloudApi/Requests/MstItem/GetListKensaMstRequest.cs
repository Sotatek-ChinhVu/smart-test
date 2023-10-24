namespace EmrCloudApi.Requests.MstItem
{
    public class GetListKensaMstRequest
    {
        public string Keyword { get; set; } = String.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
