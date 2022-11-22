namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchTenItemResponse
    {
        public SearchTenItemResponse(List<TenItemDto> listData, int totalCount)
        {
            ListData = listData;
            TotalCount = totalCount;
        }
        public int TotalCount { get; private set; }
        public List<TenItemDto> ListData { get; private set; }

    }
}
