using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.SaveSetDataTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SaveSetDataTenMstPresenter : ISaveSetDataTenMstOutputPort
    {
        public Response<SaveSetDataTenMstResponse> Result { get; private set; } = default!;

        public void Complete(SaveSetDataTenMstOutputData outputData)
        {
            Result = new Response<SaveSetDataTenMstResponse>()
            {
                Data = new SaveSetDataTenMstResponse(outputData.Status),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case SaveSetDataTenMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SaveSetDataTenMstStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case SaveSetDataTenMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SaveSetDataTenMstStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
