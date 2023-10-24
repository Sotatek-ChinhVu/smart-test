namespace EmrCloudApi.Responses.SetSendaiGeneration
{
    public class AddSetSendaiGenerationResponse
    {
        public AddSetSendaiGenerationResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; } = false;
    }
}
