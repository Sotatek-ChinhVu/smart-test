using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetCmtCheckMstList;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetCmtCheckMstListPresenter : IGetCmtCheckMstListOutputPort
    {
        public Response<GetCmtCheckMstListResponse> Result { get; private set; } = default!;

        public void Complete(GetCmtCheckMstListOutputData outputData)
        {
            Result = new Response<GetCmtCheckMstListResponse>
            {
                Data = new GetCmtCheckMstListResponse(outputData.ItemCmtModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetCmtCheckMstListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetCmtCheckMstListStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetCmtCheckMstListStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case GetCmtCheckMstListStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetCmtCheckMstListStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
