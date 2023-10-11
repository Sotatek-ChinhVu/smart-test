namespace EmrCloudApi.Responses.KensaHistory
{
    public class UpdateKensaInfDetailResponse
    {
        public UpdateKensaInfDetailResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
