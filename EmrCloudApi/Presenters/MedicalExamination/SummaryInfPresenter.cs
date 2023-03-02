using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.SummaryInf;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class SummaryInfPresenter : ISummaryInfOutputPort
    {
        public Response<SummaryInfResponse> Result { get; private set; } = default!;

        public void Complete(SummaryInfOutputData outputData)
        {
            Result = new Response<SummaryInfResponse>()
            {
                Data = new SummaryInfResponse(outputData.Header1Infos, outputData.Header2Infos, outputData.Notifications, outputData.NotificationPopUps),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SummaryInfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SummaryInfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SummaryInfStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case SummaryInfStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SummaryInfStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case SummaryInfStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case SummaryInfStatus.InvalidInfoType:
                    Result.Message = ResponseMessage.InvalidInfoType;
                    break;
                case SummaryInfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
