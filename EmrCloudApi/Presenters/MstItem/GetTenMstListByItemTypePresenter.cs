using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetTenMstListByItemType;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenMstListByItemTypePresenter : IGetTenMstListByItemTypeOutputPort
    {
        public Response<GetTenMstListByItemTypeResponse> Result { get; private set; } = default!;

        public void Complete(GetTenMstListByItemTypeOutputData outputData)
        {
            Result = new Response<GetTenMstListByItemTypeResponse>()
            {
                Data = new GetTenMstListByItemTypeResponse(outputData.TenMsts),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenMstListByItemTypeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetTenMstListByItemTypeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetTenMstListByItemTypeStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetTenMstListByItemTypeStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
