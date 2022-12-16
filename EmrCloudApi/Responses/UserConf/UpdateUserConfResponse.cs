namespace EmrCloudApi.Responses.UserConf
{
    public class UpdateUserConfResponse
    {
        public UpdateUserConfResponse(bool successed)
        {
            Successed = successed;
        }

        public bool Successed { get; private set; }
    }
}
