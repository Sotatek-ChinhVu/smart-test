namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateCmtCheckMstResponse
    {
        public UpdateCmtCheckMstResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}