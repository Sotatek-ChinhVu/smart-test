namespace EmrCloudApi.Responses.SetSendaiGeneration
{
    public class RestoreSetSendaiGenerationResponse
    {
        public RestoreSetSendaiGenerationResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; } = false;
    }
}
