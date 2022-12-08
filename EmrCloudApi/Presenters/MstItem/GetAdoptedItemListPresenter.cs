using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetAdoptedItemList;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetAdoptedItemListPresenter : IGetAdoptedItemListOutputPort
    {
        public Response<GetAdoptedItemListResponse> Result { get; private set; } = default!;

        public void Complete(GetAdoptedItemListOutputData outputData)
        {
            Result = new Response<GetAdoptedItemListResponse>
            {
                Data = new GetAdoptedItemListResponse(outputData.TenMstItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetAdoptedItemListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetAdoptedItemListStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetAdoptedItemListStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetAdoptedItemListStatus.InvalidItemCds:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetAdoptedItemListStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
