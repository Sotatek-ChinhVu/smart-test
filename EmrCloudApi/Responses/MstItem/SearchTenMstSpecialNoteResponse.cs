namespace EmrCloudApi.Responses.MstItem
{
    public class SearchTenMstSpecialNoteResponse
    {
        public SearchTenMstSpecialNoteResponse(List<TenItemDto> listData, int totalCount)
        {
            ListData = listData;
            TotalCount = totalCount;
        }
        public int TotalCount { get; private set; }
        public List<TenItemDto> ListData { get; private set; }
    }
}
