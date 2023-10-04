using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.SaveSetNameMnt;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SaveSetNameMntPresenter : ISaveSetNameMntOutputPort
    {
        public Response<SaveSetNameMntResponse> Result { get; private set; } = default!;

        public void Complete(SaveSetNameMntOutputData outputData)
        {
            Result = new Response<SaveSetNameMntResponse>()
            {
                Data = new SaveSetNameMntResponse(outputData.Result),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case SaveSetNameMntStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SaveSetNameMntStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case SaveSetNameMntStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SaveSetNameMntStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SaveSetNameMntStatus.Faild:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
