namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenOfIGEItemResponse
    {
        public GetTenOfIGEItemResponse(double tenOfIGEItem) 
        {
            TenOfIGEItem = tenOfIGEItem;
        }

        public double TenOfIGEItem { get; private set; }
    }
}
