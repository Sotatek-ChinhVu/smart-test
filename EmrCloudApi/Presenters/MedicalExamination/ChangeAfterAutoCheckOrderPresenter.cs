using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ChangeAfterAutoCheckOrderPresenter : IChangeAfterAutoCheckOrderOutputPort
    {
        public Response<ChangeAfterAutoCheckOrderResponse> Result { get; private set; } = default!;

        public void Complete(ChangeAfterAutoCheckOrderOutputData outputData)
        {
            Result = new Response<ChangeAfterAutoCheckOrderResponse>()
            {
                Data = new ChangeAfterAutoCheckOrderResponse(outputData.OdrInfItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ChangeAfterAutoCheckOrderStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ChangeAfterAutoCheckOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ChangeAfterAutoCheckOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ChangeAfterAutoCheckOrderStatus.InvalidOdrInfs:
                    Result.Message = ResponseMessage.InvalidOrderInfs;
                    break;
                case ChangeAfterAutoCheckOrderStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case ChangeAfterAutoCheckOrderStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case ChangeAfterAutoCheckOrderStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
