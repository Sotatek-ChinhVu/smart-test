namespace EmrCloudApi.Responses.UserConf
{
    public class SagakuResponse
    {
        public SagakuResponse(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}
