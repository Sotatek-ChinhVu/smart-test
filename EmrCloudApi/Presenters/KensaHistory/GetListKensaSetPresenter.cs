using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KensaHistory;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.MstItem.SearchTenMstItem;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class GetListKensaSetPresenter
    {
        public Response<GetListKensaSetResponse> Result { get; private set; } = new Response<GetListKensaSetResponse>();

        public void Complete(GetListKensaSetOutputData outputData)
        {
            Result = new Response<GetListKensaSetResponse>
            {
                Data = new GetListKensaSetResponse(outputData.KensaSets),
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
