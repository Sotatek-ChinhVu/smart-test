using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetMaxAuditTrailLogDateForPrintPresenter : IGetMaxAuditTrailLogDateForPrintOutputPort
    {
        public Response<GetMaxAuditTrailLogDateForPrintResponse> Result { get; private set; } = default!;

        public void Complete(GetMaxAuditTrailLogDateForPrintOutputData outputData)
        {
            Result = new Response<GetMaxAuditTrailLogDateForPrintResponse>()
            {
                Data = new GetMaxAuditTrailLogDateForPrintResponse(outputData.Values),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetMaxAuditTrailLogDateForPrintStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetMaxAuditTrailLogDateForPrintStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetMaxAuditTrailLogDateForPrintStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetMaxAuditTrailLogDateForPrintStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
            }
        }
    }
}
