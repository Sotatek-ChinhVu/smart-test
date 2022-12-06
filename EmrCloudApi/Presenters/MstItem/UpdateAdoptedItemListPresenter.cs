using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateAdoptedItemList;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateAdoptedItemListPresenter : IUpdateAdoptedItemListOutputPort
    {
        public Response<UpdateAdoptedItemListResponse> Result { get; private set; } = default!;

        public void Complete(UpdateAdoptedItemListOutputData outputData)
        {
            Result = new Response<UpdateAdoptedItemListResponse>
            {
                Data = new UpdateAdoptedItemListResponse(outputData.Status == UpdateAdoptedItemListStatus.Successed),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateAdoptedItemListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateAdoptedItemListStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateAdoptedItemListStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case UpdateAdoptedItemListStatus.InvalidItemCds:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case UpdateAdoptedItemListStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateAdoptedItemListStatus.InvalidValueAdopted:
                    Result.Message = ResponseMessage.InvalidAdoptedValue;
                    break;
                case UpdateAdoptedItemListStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
