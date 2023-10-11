using EmrCloudApi.Constants;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Responses;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.MstItem.SearchTenMstItem;
using UseCase.KensaHistory.GetListKensaSetDetail;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class GetListKensaSetDetailPresenter
    {
        public Response<GetListKensaSetDetailResponse> Result { get; private set; } = new Response<GetListKensaSetDetailResponse>();

        public void Complete(GetListKensaSetDetailOutputData outputData)
        {
            Result = new Response<GetListKensaSetDetailResponse>
            {
                Data = new GetListKensaSetDetailResponse(outputData.KensaSetDetails),
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
