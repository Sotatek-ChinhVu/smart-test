namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenOfHRTItemResponse
    {
        public GetTenOfHRTItemResponse(Dictionary<string, double> tenOfItem)
        {
            TenOfItem = tenOfItem;
        }

        public Dictionary<string, double> TenOfItem { get; private set; }
    }
}
