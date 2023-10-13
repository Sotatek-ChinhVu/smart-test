using EmrCloudApi.Constants;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Responses;
using UseCase.KensaHistory.GetListKensaCmtMst;
using UseCase.MstItem.SearchTenMstItem;
using UseCase.KensaHistory.GetListKensaInfDetail;
using UseCase.MstItem.SearchPostCode;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class GetListKensaInfDetailPresenter
    {
        public Response<GetListKensaInfDetailResponse> Result { get; private set; } = new Response<GetListKensaInfDetailResponse>();

        public void Complete(GetListKensaInfDetailOutputData outputData)
        {
            Result = new Response<GetListKensaInfDetailResponse>
            {
                Data = new GetListKensaInfDetailResponse(outputData.ListKensaInfDetails),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchPostCodeStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchPostCodeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
