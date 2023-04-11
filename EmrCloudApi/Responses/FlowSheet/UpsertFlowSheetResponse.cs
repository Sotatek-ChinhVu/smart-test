using UseCase.FlowSheet.Upsert;

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

    public class UpsertFlowSheetMedicalResponse
    {
        public UpsertFlowSheetMedicalResponse(UpsertFlowSheetStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public UpsertFlowSheetStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
