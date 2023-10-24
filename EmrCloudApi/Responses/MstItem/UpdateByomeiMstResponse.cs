namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateByomeiMstResponse
    {
        public UpdateByomeiMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
