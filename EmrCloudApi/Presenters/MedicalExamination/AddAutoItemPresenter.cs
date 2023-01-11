using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.AddAutoItem;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class AddAutoItemPresenter : IAddAutoItemOutputPort
    {
        public Response<AddAutoItemResponse> Result { get; private set; } = default!;

        public void Complete(AddAutoItemOutputData outputData)
        {

            Result = new Response<AddAutoItemResponse>()
            {
                Data = new AddAutoItemResponse(outputData.OdrInfItemInputDatas),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case AddAutoItemStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case AddAutoItemStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case AddAutoItemStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case AddAutoItemStatus.InvalidAddedAutoItem:
                    Result.Message = ResponseMessage.TodayOrdInvalidAddedAutoItem;
                    break;
                case AddAutoItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case AddAutoItemStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
