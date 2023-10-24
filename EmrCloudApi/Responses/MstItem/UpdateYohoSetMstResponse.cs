namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateYohoSetMstResponse
    {
        public UpdateYohoSetMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
