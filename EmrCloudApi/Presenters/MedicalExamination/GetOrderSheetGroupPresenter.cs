using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.GetOrderSheetGroup;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetOrderSheetGroupPresenter : IGetOrderSheetGroupOutputPort
    {
        public Response<GetOrderSheetGroupResponse> Result { get; private set; } = default!;

        public void Complete(GetOrderSheetGroupOutputData outputData)
        {
            Result = new Response<GetOrderSheetGroupResponse>()
            {
                Data = new GetOrderSheetGroupResponse(outputData.orderSheetItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetOrderSheetGroupStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetOrderSheetGroupStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case GetOrderSheetGroupStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetOrderSheetGroupStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
