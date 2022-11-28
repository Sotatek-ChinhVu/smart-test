namespace EmrCloudApi.Responses.FlowSheet
{
    public class UpsertFlowSheetResponse
    {
        public UpsertFlowSheetResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
