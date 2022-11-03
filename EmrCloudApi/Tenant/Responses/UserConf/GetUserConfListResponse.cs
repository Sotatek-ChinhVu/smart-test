namespace EmrCloudApi.Tenant.Responses.UserConf
{
    public class GetUserConfListResponse
    {
        public GetUserConfListResponse(List<GetUserConfItemResponse> userConfs)
        {
            UserConfs = userConfs;
        }

        public List<GetUserConfItemResponse> UserConfs { get; private set; }
    }

    public class GetUserConfItemResponse
    {
        public GetUserConfItemResponse(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }
        public int Value { get; private set; }

    }
}
