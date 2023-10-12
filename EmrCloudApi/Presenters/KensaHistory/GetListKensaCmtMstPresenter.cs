using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KensaHistory;
using UseCase.KensaHistory.GetListKensaCmtMst;
using UseCase.MstItem.SearchTenMstItem;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class GetListKensaCmtMstPresenter
    {

        public Response<GetListKensaCmtMstResponse> Result { get; private set; } = new Response<GetListKensaCmtMstResponse>();

        public void Complete(GetListKensaCmtMstOutputData outputData)
        {
            Result = new Response<GetListKensaCmtMstResponse>
            {
                Data = new GetListKensaCmtMstResponse(outputData.KensaCmtMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchTenMstItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchTenMstItemStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
