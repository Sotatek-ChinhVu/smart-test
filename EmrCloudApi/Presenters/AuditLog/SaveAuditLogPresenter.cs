using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.AuditLog;
using UseCase.SaveAuditLog;

namespace EmrCloudApi.Presenters.AuditLog
{
    public class SaveAuditLogPresenter : ISaveAuditTrailLogOutputPort
    {
        public Response<SaveAuditLogResponse> Result { get; private set; } = new Response<SaveAuditLogResponse>();
        public void Complete(SaveAuditTrailLogOutputData outputData)
        {
            Result = new Response<SaveAuditLogResponse>()
            {
                Data = new SaveAuditLogResponse(outputData.Status == SaveAuditTrailLogStatus.Successed),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private string GetMessage(SaveAuditTrailLogStatus status) => status switch
        {
            SaveAuditTrailLogStatus.Successed => ResponseMessage.Success,
            SaveAuditTrailLogStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
