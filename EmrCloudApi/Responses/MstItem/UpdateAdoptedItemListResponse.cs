namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateAdoptedItemListResponse
    {
        public UpdateAdoptedItemListResponse(bool successed)
        {
            Successed = successed;
        }
        public bool Successed { get; private set; }
    }
}
