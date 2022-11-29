namespace EmrCloudApi.Responses.SystemGenerationConf
{
    public class GetSystemGenerationConfResponse
    {
        public GetSystemGenerationConfResponse(int value, string param)
        {
            Value = value;
            Param = param;
        }

        public int Value { get; private set; }

        public string Param { get; private set; }
    }
}
