namespace EmrCloudApi.Responses.MstItem
{
    public class UpdateKensaStdMstResponse
    {
        public UpdateKensaStdMstResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
