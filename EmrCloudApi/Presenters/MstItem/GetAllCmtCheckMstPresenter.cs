using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetCmtCheckMstList;
using UseCase.MstItem.GetAllCmtCheckMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetAllCmtCheckMstPresenter
    {
        public Response<GetAllCmtCheckMstResponse> Result { get; private set; } = default!;
        public void Complete(GetAllCmtCheckMstOutputData outputData)
        {
            Result = new Response<GetAllCmtCheckMstResponse>
            {
                Data = new GetAllCmtCheckMstResponse(outputData.ItemCmtModels),
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
