namespace EmrCloudApi.Responses.KensaHistory
{
    public class UpdateKensaSetResponse
    {
        public UpdateKensaSetResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
