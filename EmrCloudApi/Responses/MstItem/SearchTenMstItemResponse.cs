namespace EmrCloudApi.Responses.MstItem
{
    public class SearchTenMstItemResponse
    {
        public SearchTenMstItemResponse(List<TenItemDto> tenMsts, int totalCount)
        {
            TenMsts = tenMsts;
            TotalCount = totalCount;
        }
        public int TotalCount { get; private set; }
        public List<TenItemDto> TenMsts { get; private set; }
    }
}
