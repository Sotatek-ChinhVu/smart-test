namespace EmrCloudApi.Responses.UserConf
{
    public class GetUserConfigParamResponse
    {
        public GetUserConfigParamResponse(string param)
        {
            Param = param;
        }

        public string Param { get; private set; }
    }
}
