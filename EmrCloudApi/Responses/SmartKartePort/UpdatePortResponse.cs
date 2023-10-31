namespace EmrCloudApi.Responses.SmartKartePort
{
    public class UpdatePortResponse
    {
        public UpdatePortResponse(bool data)
        {
            Data = data;
        }

        public bool Data { get; private set; }
    }
}
