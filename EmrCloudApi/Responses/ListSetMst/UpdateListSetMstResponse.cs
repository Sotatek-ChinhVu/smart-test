namespace EmrCloudApi.Responses.NewFolder
{
    public class UpdateListSetMstResponse
    {
        public UpdateListSetMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
