using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetKensaAuditTrailLog;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetKensaAuditLogTrailPresenter : IGetKensaAuditTrailLogOutputPort
    {
        public Response<GetKensaAuditTrailLogResponse> Result { get; private set; } = default!;

        public void Complete(GetKensaAuditTrailLogOutputData outputData)
        {

            Result = new Response<GetKensaAuditTrailLogResponse>()
            {
                Data = new GetKensaAuditTrailLogResponse(outputData.AuditTrailLogItems),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetKensaAuditTrailLogStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetKensaAuditTrailLogStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetKensaAuditTrailLogStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetKensaAuditTrailLogStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
