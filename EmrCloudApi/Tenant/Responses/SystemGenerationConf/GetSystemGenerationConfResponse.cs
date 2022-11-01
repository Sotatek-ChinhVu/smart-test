namespace EmrCloudApi.Tenant.Responses.SystemGenerationConf
{
    public class GetSystemGenerationConfResponse
    {
        public GetSystemGenerationConfResponse(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}
