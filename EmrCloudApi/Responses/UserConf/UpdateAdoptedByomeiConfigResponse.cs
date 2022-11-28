namespace EmrCloudApi.Responses.UserConf
{
    public class UpdateAdoptedByomeiConfigResponse
    {
        public UpdateAdoptedByomeiConfigResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
