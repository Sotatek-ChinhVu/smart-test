namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenOfHRTItemResponse
    {
        public GetTenOfHRTItemResponse(double tenOfHRTItem)
        {
            TenOfHRTItem = tenOfHRTItem;
        }

        public double TenOfHRTItem { get; private set; }
    }
}
