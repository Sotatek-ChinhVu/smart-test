namespace EmrCloudApi.Responses.SetSendaiGeneration
{
    public class DeleteSetSendaiGenerationResponse
    {
        public DeleteSetSendaiGenerationResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; } = false;
    }
}
