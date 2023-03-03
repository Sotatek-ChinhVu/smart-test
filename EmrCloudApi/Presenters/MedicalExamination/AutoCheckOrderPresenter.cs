using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.AutoCheckOrder;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class AutoCheckOrderPresenter : IAutoCheckOrderOutputPort
    {
        public Response<AutoCheckOrderResponse> Result { get; private set; } = default!;

        public void Complete(AutoCheckOrderOutputData outputData)
        {
            Result = new Response<AutoCheckOrderResponse>()
            {
                Data = new AutoCheckOrderResponse(outputData.AutoCheckOrderItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case AutoCheckOrderStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case AutoCheckOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case AutoCheckOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case AutoCheckOrderStatus.InvalidOdrInfs:
                    Result.Message = ResponseMessage.InvalidOrderInfs;
                    break;
                case AutoCheckOrderStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
