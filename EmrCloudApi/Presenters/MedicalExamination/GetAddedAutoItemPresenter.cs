using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetAddedAutoItem;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetAddedAutoItemPresenter : IGetAddedAutoItemOutputPort
    {
        public Response<GetAddedAutoItemResponse> Result { get; private set; } = default!;

        public void Complete(GetAddedAutoItemOutputData outputData)
        {

            Result = new Response<GetAddedAutoItemResponse>()
            {
                Data = new GetAddedAutoItemResponse(outputData.AddedAutoItems),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetAddedAutoItemStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetAddedAutoItemStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetAddedAutoItemStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetAddedAutoItemStatus.InvalidAddedAutoItem:
                    Result.Message = ResponseMessage.TodayOrdInvalidAddedAutoItem;
                    break;
                case GetAddedAutoItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetAddedAutoItemStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
